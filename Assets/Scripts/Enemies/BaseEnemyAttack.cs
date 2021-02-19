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

    public void Update()
    {
        if (coolDownTime > 0)
            coolDownTime -= Time.deltaTime;
    }

    public virtual void Attack(Destructable target)
    {
        if (coolDownTime > 0)
            return;
        
        Debug.Log("This is bad");
        target.TakeDamage(damage);
        coolDownTime = timeBetweenAttacks;
    }

    public virtual bool CanAttack(GameObject target)
    {
        if (range < Vector3.Distance(target.transform.position, transform.position))
            return false;

        var targetDirection = (target.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, targetDirection, range+1, layerMask))
            return false;

        return true;
    } 

    public virtual Destructable CalculateTarget(GameObject[] targets)
    {
        Destructable closestTarget = null;
        var d = float.MaxValue;
        foreach (var tar in targets)
        {
            Destructable destr = tar.GetComponent<Destructable>();

            if (destr == null)
                continue;

            if (!CanAttack(tar))
                continue;

            var dd = Vector3.Distance(tar.transform.position, transform.position);
            if (dd < d)
            { 
                closestTarget = destr;
                d = dd;
            }
        }
        return closestTarget;
    }
    
}