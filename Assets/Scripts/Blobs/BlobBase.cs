using DG.Tweening;
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
    [SerializeField] private float throwHeight = 6f;
    [SerializeField] private float throwSpeed = 15f;
    #endregion

    internal BlobState state;
    public BlobState State 
    {
        get
        {
            return state;
        }

        internal set
        {
            if (value != BlobState.Following)
            {
                navMeshAgent.enabled = false;
            }
            else
            {
                navMeshAgent.enabled = true;
            }

            state = value;
        }
    }

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

    public virtual void GetThrown(Vector3 startPosition, Vector3 endPosition)
    {
        transform.position = startPosition;
        State = BlobState.Flying;

        float duration = Vector3.Distance(startPosition, endPosition) / throwSpeed;
        transform.DOJump(endPosition, throwHeight, 1, duration).OnComplete(OnLanding);
    }

    public virtual void OnLanding()
    {
        State = BlobState.Idle;
        // TODO Check if there is something to interact
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
