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
    [Header("Base Movement")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float throwHeight = 6f;
    [SerializeField] private float throwSpeed = 15f;
    [Header("Base Interaction")]
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float interactionRange = 3f;
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
            if (value == BlobState.Idle)
            {
                navMeshAgent.enabled = false;
            }
            else
            {
                navMeshAgent.enabled = true;
            }

            state = value;
            InitializeState();
        }
    }

    private Transform followTarget;
    private Vector3 followOffset = Vector3.zero;
    private Vector3 lastTargetPosition = Vector3.zero;

    private Interactable currentInteractable;

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

    internal virtual void InitializeState()
    {
        switch (State)
        {
            case BlobState.Following:
                followOffset = Vector3.zero;
                break;
            case BlobState.Carrying:
                followOffset = currentInteractable.GetBlobOffset();
                followTarget = currentInteractable.transform;
                break;
        }
    }

    internal virtual void ExecuteStateBehaviour()
    {
        switch (State)
        {
            case BlobState.Following:
            case BlobState.Carrying:
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
        SearchInteractables();
    }

    internal virtual void SearchInteractables()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, interactionRange, Vector3.up, 0, interactableLayerMask);
        List<EnemyController> nearbyEnemies = new List<EnemyController>();
        List<Interactable> nearbyInteractables = new List<Interactable>();

        foreach(var hit in hits)
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                nearbyEnemies.Add(enemy);
                continue;
            }

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                nearbyInteractables.Add(interactable);
            }
        }

        if (nearbyEnemies.Count > 0)
        {
            // Attack closest enemy
            var closestEnemy = Helper.GetClosestObject(nearbyEnemies.ToArray(), transform.position);
            return;
        }

        if (nearbyInteractables.Count > 0)
        {
            // Interact with closest interactable
            currentInteractable = Helper.GetClosestObject(nearbyInteractables.ToArray(), transform.position);
            State = currentInteractable.AssignBlob(this);
            return;
        }
    }

    internal virtual void MoveTowardsFollowTarget()
    {
        Vector3 targetPosition = followTarget.position + followOffset;
        if(targetPosition != lastTargetPosition)
        {
            navMeshAgent.SetDestination(targetPosition);
            lastTargetPosition = targetPosition;
        }
    }
}
