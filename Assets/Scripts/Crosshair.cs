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
    [SerializeField] private GunController theGunController; 

    public void WalkingAnimation(bool _flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Walk", _flag);
        animator.SetBool("walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
        animator.SetBool("running", _flag);
    }

    public void JumpingAnimation(bool _flag)
    {
        animator.SetBool("running", _flag);
    }

    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("crouching", _flag);
    }

    public void FineSightAnimation(bool _flag)
    {
        animator.SetBool("fineSight", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("walking"))
            animator.SetTrigger("walk_fire");
        else if (animator.GetBool("crouching"))
            animator.SetTrigger("crouch_fire");
        else
            animator.SetTrigger("idle_fire");

    }

    public float GetAccuracy()
    {
        if (animator.GetBool("walking"))
            gunAccuracy = 0.06f;
        else if (animator.GetBool("crouching"))
            gunAccuracy = 0.015f;
        else if (theGunController.GetFineSightMode())
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.035f;

        return gunAccuracy;
    }
}

 