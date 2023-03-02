using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField] private int hp;        //체력
    [SerializeField] private GameObject go_twig_hit_effect_prefab;      // 타격 이펙트

    [SerializeField] private float destroyTime;     //이펙트 삭제 시간

    // 회전값 변수
    private Vector3 orginRot;
    private Vector3 wantedRot;
    private Vector3 currentRot;

    // 필요한 사운드 이름
    [SerializeField] private string hit_Sound;
    [SerializeField] private string broken_Sound;

    void Start()
    {
        orginRot = transform.rotation.eulerAngles;
        currentRot = orginRot;
    }

    public void Damage(Transform _playerTf)
    {
        hp--;

        Hit();

        StartCoroutine(HitSwayCoroutine(_playerTf));

        if(hp <= 0)
        {

        }
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hit_Sound);

        GameObject clone = Instantiate(go_twig_hit_effect_prefab, 
                                        gameObject.GetComponent<BoxCollider>().bounds.center,
                                        Quaternion.identity);

        Destroy(clone, destroyTime);

    }

    IEnumerator HitSwayCoroutine(Transform _target)
    {
        Vector3 direction = (_target.position - transform.position).normalized;

        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;

        CheckDirection(rotationDir);

        yield return null;  
    }

    private void CheckDirection(Vector3 _rotationDir)
    {
        Debug.Log(_rotationDir);
    }
}
