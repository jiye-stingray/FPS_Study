using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Animator animator;

    //크로스헤어 상태에 따른 총의 정확도
    private float gunAccuracy;

    //크로스헤어 비활성화를 위한 부모 객체
    [SerializeField] private GameObject go_crosshairHUD;


    // Update is called once per frame
    void Update()
    {
        
    }
}
