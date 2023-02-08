using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    //�ʿ��� ������Ʈ
    private Rigidbody rigid;
    private CapsuleCollider capsuleCollider;
    private GunController theGunController;
    private Crosshair theCrosshair;

    //���ǵ� ���� ����
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private float applySpeed;

    //���� ����
    [SerializeField] private float jumpForce; 

    // ���� ����
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // ������ üũ ����
    private Vector3 lastPos;

    //�ɾ��� �� �󸶳� �ɾ����� �����ϴ� ����
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;
    
    [Header("Camera")]
    [SerializeField] private float lookSensitivity;     //ī�޶��� �ΰ���
    [SerializeField] private float cameraRotationLimit; //ī�޶� ���� ����
    private float currentCameraRotationX = 0;
    [SerializeField] private Camera _camera;

    Vector3 _velocityVec = Vector3.zero;        //������ �ӵ� ����

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
    }

    // Start is called before the first frame update
    void Start()
    {
        applySpeed = walkSpeed;

        //�ʱ�ȭ
        originPosY = _camera. transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {

        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        MoveCheck();
        CameraRotation();
        CharacterRotation();

    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpingAnimation(!isGround);
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        //���� ���¿��� ���� �� ����
        if (isCrouch)      
            Crouch();       
        rigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        //���� ���¿��� ���� �� ����
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    /// <summary>
    /// �޸��� ���
    /// </summary>
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    /// <summary>
    /// �ɱ� �õ�
    /// </summary>
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    /// <summary>
    /// �ɱ� ����
    /// </summary>
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    /// <summary>
    /// �ε巯�� �ɱ� ���� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator CrouchCoroutine()
    {
        float _posY = _camera.transform.localPosition.y;

        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);       //����
            _camera.transform.localPosition = new Vector3(0,_posY,0 );

            if (count > 15)
                break;

            yield return null;      //1������ ����
        }

        _camera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);     //���� �� position ���߱�


        yield return new WaitForSeconds(1f);

    }
        
    private void Move()
    {
        float _h = Input.GetAxisRaw("Horizontal");
        float _v = Input.GetAxisRaw("Vertical");

        Vector3 _moveH = transform.right * _h;
        Vector3 _moveV = transform.forward * _v;

        _velocityVec = (_moveH + _moveV).normalized * applySpeed;

        rigid.MovePosition(transform.position + _velocityVec * Time.deltaTime);

    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (_velocityVec.magnitude >= 0.01f)

                isWalk = true;

            else

                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }


    }

    private void CameraRotation()
    {
        //���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        _camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        //�¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
