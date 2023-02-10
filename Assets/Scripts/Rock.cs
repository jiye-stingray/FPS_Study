using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField] private int hp;    // 바위의 체력

    [SerializeField] private float destroyTime; // 파편 제거 시간

    [SerializeField] private SphereCollider col;    // 구체 콜라이더

    // 필요한 게임 오브젝트
    [SerializeField] private GameObject go_rock;    // 일반 바위
    [SerializeField] private GameObject go_debris;  // 깨진 바위


    private void Mining()
    {
        hp--;
        if (hp <= 0)
            Destruction();
    } 

    private void Destruction()
    {
        col.enabled= false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}

