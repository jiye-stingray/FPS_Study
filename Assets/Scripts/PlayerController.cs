using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rigid;

    //���ǵ� ���� ����
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;

    // ���� ����
    private bool isRun = false;

    [Header("Camera")]
    [SerializeField] private float lookSensitivity;     //ī�޶��� �ΰ���
    [SerializeField] private float cameraRotationLimit; //ī�޶� ���� ����
    private float currentCameraRotationX = 0;
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        applySpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();

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
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _h = Input.GetAxisRaw("Horizontal");
        float _v = Input.GetAxisRaw("Vertical");

        Vector3 _moveH = transform.right * _h;
        Vector3 _moveV = transform.forward * _v;

        Vector3 _velocityVec = (_moveH + _moveV).normalized * applySpeed;

        rigid.MovePosition(transform.position + _velocityVec * Time.deltaTime);

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
