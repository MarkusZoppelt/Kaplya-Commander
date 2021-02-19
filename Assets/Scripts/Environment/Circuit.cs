using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Circuit : Interactable
{
    [Header("Circuit")]
    [SerializeField] private ConductorPlate[] conductorPlates;
    [SerializeField] private UnityEvent onCircuitComplete;
    [SerializeField] private UnityEvent onCircuitBroken;

    private Dictionary<BlobBase, ConductorPlate> occupiedPlates = new Dictionary<BlobBase, ConductorPlate>();

    public override bool CanBeAssigned(BlobBase blob)
    {
        if (assignedBlobs.Count >= conductorPlates.Length)
        {
            return false;
        }

        return blob.GetType() == typeof(ConductorBlob);
    }

    internal override void StartInteraction()
    {
        base.StartInteraction();
        onCircuitComplete?.Invoke();
    }

    internal override void StopInteraction()
    {
        base.StopInteraction();
        onCircuitBroken?.Invoke();
    }

    public override BlobState AssignBlob(BlobBase blob)
    {
        var state = base.AssignBlob(blob);
        blob.StartInteracting(this);
        return state;
    }

    public override void RemoveBlob(BlobBase blob)
    {
        base.RemoveBlob(blob);
        if (occupiedPlates.ContainsKey(blob))
        {
            occupiedPlates[blob].IsOccupied = false;
            occupiedPlates.Remove(blob);
        }
    }

    public override Vector3 GetBlobOffset(BlobBase blob)
    {
        if (occupiedPlates.ContainsKey(blob))
        {
            return occupiedPlates[blob].transform.localPosition;
        }

        foreach (var plate in conductorPlates)
        {
            if (!plate.IsOccupied)
            {
                plate.IsOccupied = true;
                occupiedPlates.Add(blob, plate);
                return plate.transform.localPosition;
            }
        }

        Debug.LogWarning("Trying to assign more blobs to a circuit than there are plates");
        return Vector3.zero;
    }
}
