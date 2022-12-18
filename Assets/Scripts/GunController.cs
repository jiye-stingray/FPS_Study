using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;

    private float currentFireRate;

    private bool isReload = false;
    private bool isfineSightMode = false;       //������ ���� bool ����

    [SerializeField] private Vector3 originPos;     //���� ������ ��

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
            currentFireRate -= Time.deltaTime;  //60���� 1 = 1
    }

    private void TryFire()
    {

        if (Input.GetMouseButtonDown(0) && currentFireRate <= 0 && !isReload)
        {
            Debug.Log("Try");
            Fire();
        }
    }

    private void Fire()
    {
        if (isReload) return;

        if (currentGun.currentBulletCount > 0)
        {
            Shoot();
        }
        else
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;      //���� �ӵ� ����
        PlaySE(currentGun.fire_Sound); 
        currentGun.muzzleFlesh.Play();

        //�ѱ� �ݵ� �ڷ�ƾ ����

        Debug.Log("�Ѿ� �߻���");
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount +=  currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;    
            }

            isReload = false;
        }
        else
        {
            Debug.Log("������ �Ѿ��� �����ϴ�");
        }
    }

    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            FineSight();
        }
    }

    private void FineSight()
    {
        isfineSightMode = !isfineSightMode;

        currentGun.anim.SetBool("FineSightMode",isfineSightMode);

        if (isfineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()
    {
        //Lerp ���� ����� ���� position ���� �� �¾� �������� �ʾƼ� 
        // �Ʒ� while �� ������ ������� �ʴ´�. (�ڷ�ƾ�� �������� �۵�)
        // ���� �� �ڷ�ƾ�� ������ �� �ٸ� �ڷ�ƾ���� ���� ����� �Ѵ�.
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos,0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun .retroActionForce,originPos.y,originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y,currentGun.fineSightOriginPos.z);

        if (!isfineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            //�ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);

                yield return null;
            }

            //�� ��ġ

        }
    }


    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
