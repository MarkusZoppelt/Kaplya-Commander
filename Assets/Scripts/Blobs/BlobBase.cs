using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BlobState
{
    Imprisoned,
    Idle,
    Flying,
    Following,
    Carrying,
    Fighting
}

public class BlobBase : MonoBehaviour
{
    #region Inspector
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float movementSpeed = 8f;
    #endregion
    public BlobState State { get; internal set; }

    private Transform followTarget;
    private Vector3 lastTargetPosition = Vector3.zero;

    #region Unity methods
    internal virtual void Awake()
    {
        InitializeBaseBlob();
    }

    internal virtual void Update()
    {
        ExecuteStateBehaviour();
    }
    #endregion

    internal void InitializeBaseBlob()
    {
        State = BlobState.Idle;
        navMeshAgent.speed = movementSpeed;
    }

    internal virtual void ExecuteStateBehaviour()
    {
        switch (State)
        {
            case BlobState.Following:
                MoveTowardsFollowTarget();
                break;
            default:
                break;
        }
    }

    public virtual void StartFollowing(Transform target)
    {
        State = BlobState.Following;
        followTarget = target;
    }

    internal virtual void MoveTowardsFollowTarget()
    {
        if(followTarget.position != lastTargetPosition)
        {
            navMeshAgent.SetDestination(followTarget.position);
            lastTargetPosition = followTarget.position;
        }
    }
}
