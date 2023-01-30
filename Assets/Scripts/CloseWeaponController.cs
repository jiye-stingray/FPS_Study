using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{       // �̿ϼ� Ŭ���� = �߻� Ŭ����

    // Ȱ��ȭ ����
    public static bool isActivate = false;

    // ���� ������ Hand�� Ÿ�� ����
    [SerializeField] protected CloseWeapon currentHand;

    // ������ ??
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {

            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }

        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isSwing = false;

    }

    // �̿ϼ� = �߻� �ڷ�ƾ
    protected abstract IEnumerator HitCoroutine();


    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;

    }

    public void HandChange(CloseWeapon _hand)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = UnityEngine.Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;


    }
}
