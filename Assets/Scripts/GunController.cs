using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // �ʿ��� ������Ʈ
    [SerializeField] private Camera theCam;
    private Crosshair theCrosshair;

    // ���� ������ ��
    [SerializeField] private Gun currentGun;

    // ���� �ӵ� ���
    private float currentFireRate;

    // ���� ����
    private bool isReload = false;
    [HideInInspector] public bool isfineSightMode = false;       //������ ���� bool ����

    //���� ������ ��
    private Vector3 originPos;

    //ȿ���� ���
    private AudioSource audioSource;

    // ������ �浹 ���� �޾ƿ�
    private RaycastHit hitInfo;

    // �ǰ� ����Ʈ
    [SerializeField] private GameObject hit_effect_prefab;

    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<Crosshair>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    // ����ӵ� ����
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;  //60���� 1 = 1
    }

    // �߻� �ӵ�
    private void TryFire()
    {

        if (Input.GetMouseButtonDown(0) && currentFireRate <= 0 && !isReload)
        {
            Debug.Log("Try");
            Fire();
        }
    }

    // �߻� �� ���
    private void Fire()
    {
        if (isReload) return;

        if (currentGun.currentBulletCount > 0)
        {
            Shoot();
        }
        else
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    // �߻� �� ���
    private void Shoot()
    {
        Debug.Log("Shoot");

        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;      //���� �ӵ� ����
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlesh.Play();
        Hit();
        //�ѱ� �ݵ� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

    }

    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo, currentGun.range))
        {
            GameObject clone =  Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f); 
        }
    }

    // ������ �õ�
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    // ������
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
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

    // ������ �õ�
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // ������ ���
    public void CancelFineSight()
    {
        if (isfineSightMode)
            FineSight();
    }

    // ������ ���� ���� 
    private void FineSight()
    {
        isfineSightMode = !isfineSightMode;

        currentGun.anim.SetBool("FineSightMode", isfineSightMode);

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

    //������ Ȱ��ȭ
    IEnumerator FineSightActivateCoroutine()
    {
        //Lerp ���� ����� ���� position ���� �� �¾� �������� �ʾƼ� 
        // �Ʒ� while �� ������ ������� �ʴ´�. (�ڷ�ƾ�� �������� �۵�)
        // ���� �� �ڷ�ƾ�� ������ �� �ٸ� �ڷ�ƾ���� ���� ����� �Ѵ�.
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    // ������ �� Ȱ��ȭ
    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // �ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isfineSightMode)
        {
            //������ ��ġ�϶� �ݵ��� ū ���̰� �����Ƿ� ���� ��ġ�� �������� �ݵ��� ����
            currentGun.transform.localPosition = originPos;

            //�ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);

                yield return null;
            }

            // ����ġ
            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            //�ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);

                yield return null;
            }

            // ����ġ
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
            }
        }
    }

    // ���� ���
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }
}
