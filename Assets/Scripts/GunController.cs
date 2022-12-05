using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;

    private float currentFireRate;

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

        if (Input.GetMouseButtonDown(0) && currentFireRate <= 0)
        {
            Debug.Log("Try");
            Fire();
        }
    }

    private void Fire()
    {

        if (currentGun.currentBulletCount > 0)
        {
            Shoot();
        }
        else
        {
            Reload();
        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;      //연사 속도 재계산
        PlaySE(currentGun.fire_Sound); 
        currentGun.muzzleFlesh.Play(); 
    }

    private void Reload()
    {
        if(currentGun.carryBulletCount > 0)
        {
            currentGun.anim.SetTrigger("Reload");

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
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
