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
    Fighting,
    Interacting,
    GoingToTube
}

public class BlobBase : MonoBehaviour
{
    #region Inspector
    [Header("Base Movement")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float defaultProximity = 1f;
    [SerializeField] private float throwHeight = 6f;
    [SerializeField] private float throwSpeed = 15f;
    [Header("Base Interaction")]
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private float carryProximity = 0.1f;
    #endregion

    internal BlobState state;
    public BlobState State 
    {
        get
        {
            return state;
        }

        set
        {
            if (value == BlobState.Idle || value == BlobState.Carrying)
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

    public virtual bool CanBeCalled()
    {
        if (State == BlobState.Imprisoned || State == BlobState.Flying || State == BlobState.Following || State == BlobState.GoingToTube)
        {
            return false;
        }

        return true;
    }

    #region Orchestrate state behaviour
    internal virtual void InitializeState()
    {
        switch (State)
        {
            case BlobState.GoingToTube:
            case BlobState.Following:
                InitializeFollowing();
                break;
            case BlobState.Carrying:
                InitializeCarrying();
                break;
            case BlobState.Interacting:
                InitializeInteracting();
                break;
        }
    }

    internal virtual void ExecuteStateBehaviour()
    {
        switch (State)
        {
            case BlobState.GoingToTube:
            case BlobState.Following:
                ExecuteFollowing();
                break;
            case BlobState.Carrying:
                ExecuteCarrying();
                break;
            case BlobState.Interacting:
                ExecuteInteracting();
                break;
            default:
                break;
        }
    }
    #endregion

    #region Following
    public virtual void StartFollowing(Transform target)
    {
        State = BlobState.Following;
        followTarget = target;
    }

    internal virtual void InitializeFollowing()
    {
        if(currentInteractable != null)
        {
            currentInteractable.RemoveBlob(this);
            currentInteractable = null;
        }

        followOffset = Vector3.zero;
        navMeshAgent.stoppingDistance = defaultProximity;

        transform.DOJump(transform.position, 0.75f, 1, 0.33f);
    }

    internal virtual void ExecuteFollowing()
    {
        MoveTowardsFollowTarget();
    }
    #endregion

    #region Carrying
    internal virtual void InitializeCarrying()
    {
        transform.DOJump(transform.position, 0.75f, 1, 0.33f);
    }

    internal virtual void ExecuteCarrying() { }
    #endregion

    #region Interacting
    public virtual void StartInteracting(Interactable target)
    {
        State = BlobState.Interacting;
        currentInteractable = target;
    }

    internal virtual void InitializeInteracting()
    {
        followTarget = currentInteractable.transform;
        followOffset = currentInteractable.GetBlobOffset();

        transform.DOJump(transform.position, 0.75f, 1, 0.33f);
    }

    internal virtual void ExecuteInteracting()
    {
        MoveTowardsFollowTarget();
    }
    #endregion

    #region Getting Thrown
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
            if (interactable != null && interactable.CanBeAssigned(this))
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
    #endregion

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
