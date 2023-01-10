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


    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("walking", _flag);
    }

    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("running", _flag);
    }

    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("crouching", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("walking"))
            animator.SetTrigger("walk_fire");
        else if (animator.GetBool("crouching"))
            animator.SetTrigger("crouch_fire");
        else
        {
            animator.SetTrigger("idle_fire");
        }

    }
}

 