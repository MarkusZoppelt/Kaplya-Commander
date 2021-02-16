using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CarryObject : Interactable
{
    [SerializeField] internal NavMeshAgent agent;
    [SerializeField] internal float carrySpeed = 4f;
    [SerializeField] internal UnityEvent onSuckedIntoTube;

    public RoomManager Room { private get; set; }

    internal override void StartInteraction()
    {
        if(Room == null)
        {
            return;
        }

        agent.SetDestination(Room.Tube.transform.position);
    }

    public override BlobState AssignBlob(BlobBase blob)
    {
        var state = base.AssignBlob(blob); 
        UpdateCarrySpeed();
        blob.transform.SetParent(transform);
        return state;
    }

    public override void RemoveBlob(BlobBase blob)
    {
        base.RemoveBlob(blob);
        blob.transform.SetParent(null);
    }

    internal void UpdateCarrySpeed()
    {
        int numberOfAssigendBlobs = assignedBlobs.Count;
        if (numberOfAssigendBlobs < blobsNeeded)
        {
            agent.speed = 0f;
            return;
        }

        agent.speed = carrySpeed + ((numberOfAssigendBlobs - blobsNeeded) * 0.2f * carrySpeed);
    }

    public void OnSuckedIntoTube()
    {
        foreach(var blob in assignedBlobs.ToArray())
        {
            RemoveBlob(blob);
        }
        onSuckedIntoTube?.Invoke();
    }
}
