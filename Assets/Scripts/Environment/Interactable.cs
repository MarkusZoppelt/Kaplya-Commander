using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region Inspector
    [Header("Base Interactable")]
    [SerializeField] internal int blobsNeeded;
    [SerializeField] internal float radius;
    [SerializeField] public BlobState interactorState;
    #endregion

    internal List<BlobBase> assignedBlobs = new List<BlobBase>();

    public virtual bool CanBeAssigned(BlobBase blob)
    {
        return true;
    }

    public virtual BlobState AssignBlob(BlobBase blob)
    {
        assignedBlobs.Add(blob);
        if (assignedBlobs.Count >= blobsNeeded)
        {
            StartInteraction();
        }

        return interactorState;
    }

    public virtual void RemoveBlob(BlobBase blob)
    {
        assignedBlobs.Remove(blob);
        blob.State = BlobState.Idle;
        if (assignedBlobs.Count < blobsNeeded)
        {
            StopInteraction();
        }
    }

    internal virtual void StartInteraction()
    {
        Debug.LogWarning($"StartInteraction for {gameObject.name} not implemented");
    }

    internal virtual void StopInteraction()
    {
        Debug.LogWarning($"StopInteraction for {gameObject.name} not implemented");
    }

    public virtual Vector3 GetBlobOffset()
    {
        float angle = assignedBlobs.Count * Mathf.PI * 2f / blobsNeeded;
        return new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
    }
}
