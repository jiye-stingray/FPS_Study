using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Timeline;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField] private int hp;        //ü��
    [SerializeField] private GameObject go_twig_hit_effect_prefab;      // Ÿ�� ����Ʈ

    [SerializeField] private float destroyTime;     //����Ʈ ���� �ð�

    // ȸ���� ����
    private Vector3 originRot;
    private Vector3 wantedRot;
    private Vector3 currentRot;

    // �ʿ��� ���� �̸�
    [SerializeField] private string hit_Sound;
    [SerializeField] private string broken_Sound;

    void Start()
    {
        originRot = transform.rotation.eulerAngles;
        currentRot = originRot;
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

        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.25f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;  
        }

        wantedRot = originRot;

        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.15f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }

    }

    private bool CheckThreshold()
    {
        if (Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f && Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f)
            return true;
        return false;
    }


    private void CheckDirection(Vector3 _rotationDir)
    {
        Debug.Log(_rotationDir);

        if(_rotationDir.y > 180)
        {
            if (_rotationDir.y > 300)
                wantedRot = new Vector3(-50f,0f,-50f) ;
            else if (_rotationDir.y > 240)
                wantedRot = new Vector3(0f, 0f, -50f);
            else
                wantedRot = new Vector3(50f, 0f, -50f);
        }
        else if (_rotationDir.y <= 180)
        {
            if (_rotationDir.y < 60)
                wantedRot = new Vector3(-50f, 0f, -50f);
            else if (_rotationDir.y > 120)
                wantedRot = new Vector3(0f, 0f, 50f);
            else
                wantedRot = new Vector3(50f, 0f, 50f);
        }
    }
}
