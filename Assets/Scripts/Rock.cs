using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField] private int hp;    // ������ ü��

    [SerializeField] private float destroyTime; // ���� ���� �ð�

    [SerializeField] private SphereCollider col;    // ��ü �ݶ��̴�

    // �ʿ��� ���� ������Ʈ
    [SerializeField] private GameObject go_rock;    // �Ϲ� ����
    [SerializeField] private GameObject go_debris;  // ���� ����
    [SerializeField] private GameObject go_effect_prefabs;  // ä�� ����Ʈ

    public void Mining()
    {
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;

        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}

