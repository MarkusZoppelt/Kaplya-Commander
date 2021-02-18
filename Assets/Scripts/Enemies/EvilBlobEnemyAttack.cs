using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilBlobEnemyAttack : BaseEnemyAttack
{
    [SerializeField] internal bool isFreed;

    public override void Attack(GameObject target)
    {
        Debug.LogWarning("Ambush Player (not implemented yet)");
    }

    // only attack player when freed and very close
    public override bool CanAttack(GameObject target)
    {
        if (!isFreed)
            return false;

        if (range > Vector3.Distance(target.transform.position, transform.position))
            return false;

        var targetDirection = (target.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, targetDirection, range, layerMask))
            return false;

        return true;
    }
}
