using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{
    // 무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    // 현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기의 타입
    [SerializeField] private string currentWeaponType;

    // 무기 교체 딜레이 , 무기 교체가 완전히 끝난 시점
    [SerializeField] private float changeWeaponDelayTime;
    [SerializeField] private float changeWeaponEndDelayTime;

    // 무기 종류를 전부 관리
    [SerializeField] private Gun[] guns;
    [SerializeField] private Hand[] hands;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();   
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    // 필요한 컴포넌트
    [SerializeField] private GunController theGunController;
    [SerializeField] private HandController theHandController; 
    


    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName,guns[i]);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
