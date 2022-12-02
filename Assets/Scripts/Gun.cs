using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public string gunName;  //���� �̸�
    public float range;     //���� �Ÿ�
    public float accuracy;  //��Ȯ��
    public float fireRate;  //���� �ӵ�
    public float reloadTime;//������ �ӵ�

    public int damage;      //���� ������    

    public int reloadBulletCount;   //���� ������ ����
    public int currentBulletCount;  //���� ź������ �����ִ� �Ѿ��� ����
    public int maxBulletCount;      //�ִ� ���� ���� �Ѿ� ����
    public int carryBulletCount;    //���� �����ϰ� �ִ� �Ѿ� ����

    public float retroActionForce;  //�ݵ� ����
    public float retroActionFineSightForce; //�����ؽ��� �ݵ� ����

    public Vector3 fineSightOriginPos;      //�����ؽ� ������ ��ġ
    public Animator anim;
    public ParticleSystem muzzleFlesh;

    public AudioClip fire_Sound;
 
}
