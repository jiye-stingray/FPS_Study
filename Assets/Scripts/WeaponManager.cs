using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{
    // ���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon = false;

    // ���� ����� ���� ������ �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // ���� ������ Ÿ��
    [SerializeField] private string currentWeaponType;

    // ���� ��ü ������ , ���� ��ü�� ������ ���� ����
    [SerializeField] private float changeWeaponDelayTime;
    [SerializeField] private float changeWeaponEndDelayTime;

    // ���� ������ ���� ����
    [SerializeField] private Gun[] guns;
    [SerializeField] private Hand[] hands;

    // ���� �������� ���� ���� ������ �����ϵ��� ����
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    // �ʿ��� ������Ʈ
    [SerializeField] private GunController theGunController;
    [SerializeField] private HandController theHandController;



    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);

        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

            }
        }
    }
}
