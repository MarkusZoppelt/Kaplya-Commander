using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerActionState
{
    NoAction,
    Throwing,
    CallingBack
}

public class PlayerController : MonoBehaviour
{
    #region Inspector
    [Header("Movement")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float gravity = -9.81f;
    [Header("Targeted Actions")]
    [SerializeField] private GameObject targetIndicator;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float minThrowDistance = 4f;
    [SerializeField] private float maxThrowDistance = 15f;
    [Header("Throwing")]
    [SerializeField] private float timeBetweenThrows = 0.5f;
    [Header("Calling back")]
    [SerializeField] private GameObject callIndicator;
    [SerializeField] private float maxCallRange = 5f;
    [SerializeField] private float rangeGrowthPerSeconds = 5f;
    [Header("Blobs")]
    [SerializeField] private LayerMask blobLayerMask;
    [SerializeField] private Transform followerTarget;
    [SerializeField] private int maxFollowers = 20;
    #endregion

    // Current player state
    private PlayerActionState state;
    private Coroutine currentActionRoutine;

    // Variables for determining where the user clicked/tapped
    private Camera playerCamera;
    private Ray targetRay;
    private RaycastHit targetHit;

    // Movement
    private Vector3 movementDirection;

    // Calling blobs back
    RaycastHit[] nearbyBlobs;

    // Throwing blobs
    private Vector3 throwTargetPosition;
    private List<BlobBase> followingBlobs;

    #region Unity Methods
    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        Move();
        UpdateTargetIndicator();
    }
    #endregion

    #region Input Events
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
        movementDirection = new Vector3(inputMovement.x, 0f, inputMovement.y);
    }

    public void OnTargetedAction(InputAction.CallbackContext context)
    {
        // TODO: Here we should also somehow determine the screen position for a touch...
        if (context.action.phase == InputActionPhase.Started)
        {
            HandleTargetedActionStart(Mouse.current.position.ReadValue());
        }
        else if (context.action.phase == InputActionPhase.Canceled)
        {
            StopCurrentAction();
        }
        
    }
    #endregion

    private void Initialize()
    {
        playerCamera = Camera.main;
        state = PlayerActionState.NoAction;
        followingBlobs = new List<BlobBase>();
    }

    private void Move()
    {
        if (state != PlayerActionState.NoAction || movementDirection.magnitude < 0.1f)
        {
            return;
        }

        Vector3 movement = movementDirection * speed * Time.deltaTime;
        movement.y = gravity;
        controller.Move(movement);

        float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    private void UpdateTargetIndicator()
    {
        targetRay = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(targetRay, out targetHit, 1000f, targetLayerMask))
        {
            throwTargetPosition = Vector3.negativeInfinity;
            targetIndicator.SetActive(false);
            return;
        }

        throwTargetPosition = targetHit.point;
        float targetDistance = Vector3.Distance(transform.position, throwTargetPosition);
        if (targetDistance < minThrowDistance)
        {
            throwTargetPosition = Vector3.negativeInfinity;
            // TODO Maybe change indicator to show that you can call blobs back from that mouse position?
            targetIndicator.SetActive(false);
            return;
        }

        if (targetDistance > maxThrowDistance)
        {
            Vector3 throwDirection = (throwTargetPosition - transform.position).normalized;
            throwTargetPosition = transform.position + (throwDirection * maxThrowDistance);
        }

        targetIndicator.SetActive(true);
        targetIndicator.transform.position = throwTargetPosition;
    }

    /// <summary>
    /// Determines the target of the action and calls either the throw- or callback-method
    /// </summary>
    private void HandleTargetedActionStart(Vector2 screenTarget)
    {
        targetRay = playerCamera.ScreenPointToRay(screenTarget);
        if (!Physics.Raycast(targetRay, out targetHit, 1000f, targetLayerMask))
        {
            return;
        }

        float targetDistance = Vector3.Distance(transform.position, targetHit.point);
        if (targetDistance < minThrowDistance)
        {
            StartCallingBlobsBack();
            return;
        }

        StartThrowingBlobs();
    }

    private void StopCurrentAction()
    {
        if (currentActionRoutine != null)
        {
            StopCoroutine(currentActionRoutine);
            currentActionRoutine = null;
        }

        switch (state)
        {
            case PlayerActionState.CallingBack:
                callIndicator.SetActive(false);
                break;
        }
        state = PlayerActionState.NoAction;
    }

    private void StartCallingBlobsBack()
    {
        StopCurrentAction();
        state = PlayerActionState.CallingBack;
        callIndicator.SetActive(true);
        currentActionRoutine = StartCoroutine(CallBlobsBack());
    }

    private IEnumerator CallBlobsBack()
    {
        float callRange = 0f;

        while (true)
        {
            callRange += rangeGrowthPerSeconds * Time.deltaTime;
            callRange = Mathf.Clamp(callRange, 0f, maxCallRange);

            nearbyBlobs = Physics.SphereCastAll(transform.position, callRange, Vector3.up, 0f, blobLayerMask);
            foreach(var blobHit in nearbyBlobs)
            {
                BlobBase blob = blobHit.collider.GetComponent<BlobBase>();
                if (blob == null || blob.State != BlobState.Idle)
                {
                    continue;
                }

                if (followingBlobs.Count < maxFollowers)
                {
                    blob.StartFollowing(followerTarget);
                    followingBlobs.Add(blob);
                }
            }

            callIndicator.transform.localScale = new Vector3(callRange, callRange, callRange);
            yield return null;
        }
    }

    private void StartThrowingBlobs()
    {
        StopCurrentAction();
        state = PlayerActionState.Throwing;
        currentActionRoutine = StartCoroutine(ThrowBlobs());
    }

    private IEnumerator ThrowBlobs()
    {
        while(followingBlobs.Count > 0)
        {
            if (throwTargetPosition == Vector3.negativeInfinity)
            {
                yield return null;
            }

            BlobBase blob = followingBlobs.First();
            followingBlobs.Remove(blob);

            blob.GetThrown(transform.position, throwTargetPosition);

            yield return new WaitForSeconds(timeBetweenThrows);
        }

        StopCurrentAction();
    }
}
