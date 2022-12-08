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
    }

    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
            currentFireRate -= Time.deltaTime;  //60분의 1 = 1
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
        currentFireRate = currentGun.fireRate;      //연사 속도 재계산
        PlaySE(currentGun.fire_Sound); 
        currentGun.muzzleFlesh.Play(); 
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
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
