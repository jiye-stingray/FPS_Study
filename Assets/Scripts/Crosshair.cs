using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //ũ�ν���� ���¿� ���� ���� ��Ȯ��
    private float gunAccuracy;

    //ũ�ν���� ��Ȱ��ȭ�� ���� �θ� ��ü
    [SerializeField] private GameObject go_crosshairHUD;


    // Update is called once per frame
    void Update()
    {
        
    }
}
