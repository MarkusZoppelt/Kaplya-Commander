using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CarryObject : Interactable
{
    [SerializeField] internal PneumaticTube carryTo;
    [SerializeField] internal NavMeshAgent agent;
    [SerializeField] internal float carrySpeed = 4f;
    [SerializeField] internal UnityEvent onSuckedIntoTube;

    internal override void StartInteraction()
    {
        agent.SetDestination(carryTo.transform.position);
    }

    public override BlobState AssignBlob(BlobBase blob)
    {
        var state = base.AssignBlob(blob); 
        UpdateCarrySpeed();
        return state;
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
