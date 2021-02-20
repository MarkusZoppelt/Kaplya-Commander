using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyMovement : MonoBehaviour
{
    [SerializeField] internal NavMeshAgent agentSmith;
    [SerializeField] internal float speed;
    [SerializeField] internal Animator enemyAnimator;
    [SerializeField] internal SpriteRenderer enemySprite;

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
        if(enemyAnimator != null) {
            enemyAnimator.SetBool("isWalking", true);
        }
        if(enemySprite != null) {
            if(pos.x < transform.position.x) {
                enemySprite.flipX = true;
            } else {
                enemySprite.flipX = false;
            }
        }
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
        if (enemyAnimator != null) {
            enemyAnimator.SetBool("isWalking", false);
        }
    }
}
