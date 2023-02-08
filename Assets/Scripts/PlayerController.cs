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
    //필요한 컴포넌트
    private Rigidbody rigid;
    private CapsuleCollider capsuleCollider;
    private GunController theGunController;
    private Crosshair theCrosshair;

    //스피드 조정 변수
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;
    private float applySpeed;

    //점프 변수
    [SerializeField] private float jumpForce; 

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // 움직임 체크 변수
    private Vector3 lastPos;

    //앉았을 때 얼마나 앉았을지 결정하는 변수
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;
    
    [Header("Camera")]
    [SerializeField] private float lookSensitivity;     //카메라의 민감도
    [SerializeField] private float cameraRotationLimit; //카메라 각도 제한
    private float currentCameraRotationX = 0;
    [SerializeField] private Camera _camera;

    Vector3 _velocityVec = Vector3.zero;        //움직임 속도 변수

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

        //초기화
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
        //앉은 상태에서 점프 시 해제
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
        //앉은 상태에서 점프 시 해제
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    /// <summary>
    /// 달리기 취소
    /// </summary>
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    /// <summary>
    /// 앉기 시도
    /// </summary>
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    /// <summary>
    /// 앉기 동작
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
    /// 부드러운 앉기 동작 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator CrouchCoroutine()
    {
        float _posY = _camera.transform.localPosition.y;

        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);       //보간
            _camera.transform.localPosition = new Vector3(0,_posY,0 );

            if (count > 15)
                break;

            yield return null;      //1프레임 마다
        }

        _camera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);     //보간 후 position 맞추기


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
        //상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        _camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        //좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
