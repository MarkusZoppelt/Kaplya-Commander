using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CarryObject : Interactable
{
    [SerializeField] internal PneumaticTube carryTo;
    [SerializeField] internal NavMeshAgent agent;
    [SerializeField] internal UnityEvent onSuckedIntoTube;

    internal override void StartInteraction()
    {
        agent.SetDestination(carryTo.transform.position);
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
