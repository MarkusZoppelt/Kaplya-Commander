using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyActionState
{
    NoAction,
    Attacking,
    RunningAtPlayer
}

public class EnemyController : Interactable
{
    [SerializeField] internal BaseEnemyAttack attack;
    [SerializeField] internal BaseEnemyMovement movement;
    [SerializeField] internal BaseEnemyVision vision;
    [SerializeField] internal Destructable destructable;
    internal GameObject[] targets;

    public EnemyActionState State { get; internal set; }
    
    private void Update()
    {
        targets = vision.AcquireTargets();
        
        if (targets.Length < 1)
            return;

        
        var movementPosition = movement.CalculateTargetPosition(targets);
        movement.MoveTowards(movementPosition);

        var tar = attack.CalculateTarget(targets);
        if (tar == null)
            return;

        attack.Attack(tar);
    }

    public override BlobState AssignBlob(BlobBase blob)
    {
        var state = base.AssignBlob(blob);
        blob.StartFighting(destructable);
        return state;
    }
}