using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyMovement : MonoBehaviour
{
    [SerializeField] internal NavMeshAgent agentSmith;
    [SerializeField] internal float speed;

    public virtual void Awake()
    {
        // Mr. Anderson!
        agentSmith.speed = speed;

        // Darn you, Mr Anderson!
        agentSmith.enabled = false;
    }

    public virtual void MoveTowards(Vector3 pos)
    {
        agentSmith.enabled = true;
        agentSmith.SetDestination(pos);
    }

    public virtual Vector3 CalculateTargetPosition(GameObject[] targets)
    {
        var closestTarget = targets[0];
        var d = float.MaxValue;
        foreach (var tar in targets)
        {
            var dd = Vector3.Distance(tar.transform.position, transform.position);
            if (dd < d)
            {
                closestTarget = tar;
                d = dd;
            }
            
        }
        return closestTarget.transform.position;
    }

    public virtual void StopMoving()
    {
        agentSmith.enabled = false;
    }
}
