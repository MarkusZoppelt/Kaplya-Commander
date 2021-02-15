using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAttack : MonoBehaviour
{
    [SerializeField] internal float damage;
    [SerializeField] internal float range;
    [SerializeField] internal float timeBetweenAttacks;
    [SerializeField] internal LayerMask layerMask;
    internal float coolDownTime;

    public virtual void Attack(GameObject target)
    {
        Debug.LogWarning("Attack not implemented");
    }

    public virtual bool CanAttack(GameObject target)
    {
        if (coolDownTime > 0)
            return false; 
        
        if (range > Vector3.Distance(target.transform.position, transform.position))
            return false;

        var targetDirection = (target.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, targetDirection, range+1, layerMask))
            return false;

        return true;
    } 

    public virtual GameObject CalculateTarget(GameObject[] targets)
    {
        GameObject closestTarget = null;
        var d = float.MaxValue;
        foreach (var tar in targets)
        {
            if (!CanAttack(tar))
                continue;

            var dd = Vector3.Distance(tar.transform.position, transform.position);
            if (dd < d)
            {
                closestTarget = tar;
                d = dd;
            }
        }
        return closestTarget;
    }
    
}