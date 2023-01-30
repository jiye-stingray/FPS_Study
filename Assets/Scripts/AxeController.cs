using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class AxeController : CloseWeaponController
{
    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if(CheckObject())
            {
                isSwing= false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
}
