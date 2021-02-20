using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region Inspector
    [Header("Base Interactable")]
    [SerializeField] internal InteractableDisplay display;
    [SerializeField] internal int blobsNeeded;
    [SerializeField] internal float radius;
    [SerializeField] public BlobState interactorState;
    #endregion

    internal bool InteractionHasStarted { get; set; }

    internal List<BlobBase> assignedBlobs = new List<BlobBase>();

    internal virtual void Awake()
    {
        display.NeededBlobs = blobsNeeded;
    }

    public virtual bool CanBeAssigned(BlobBase blob)
    {
        return true;
    }

    public virtual BlobState AssignBlob(BlobBase blob)
    {
        assignedBlobs.Add(blob);
        display.AssignedBlobs = assignedBlobs.Count;
        if (assignedBlobs.Count >= blobsNeeded && !InteractionHasStarted)
        {
            StartInteraction();
        }

        return interactorState;
    }

    public virtual void RemoveBlob(BlobBase blob)
    {
        assignedBlobs.Remove(blob);
        display.AssignedBlobs = assignedBlobs.Count;
        blob.State = BlobState.Idle;
        if (assignedBlobs.Count < blobsNeeded && InteractionHasStarted)
        {
            StopInteraction();
        }
    }

    internal virtual void StartInteraction()
    {
        InteractionHasStarted = true;
    }

    internal virtual void StopInteraction()
    {
        InteractionHasStarted = false;
    }

    public virtual Vector3 GetBlobOffset(BlobBase blob)
    {
        float angle = assignedBlobs.Count * Mathf.PI * 2f / blobsNeeded;
        return new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
    }
}
