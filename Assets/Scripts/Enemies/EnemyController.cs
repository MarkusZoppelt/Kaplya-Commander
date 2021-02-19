using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyActionState
{
    NoAction,
    Attacking,
    RunningAtPlayer
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] internal TurretEnemyAttack attack;
    [SerializeField] internal BaseEnemyMovement movement;
    [SerializeField] internal BaseEnemyVision vision;
    internal GameObject[] targets;

    public EnemyActionState State { get; internal set; }
    
    private void Update()
    {
        targets = vision.AcquireTargets();
        
        if (targets.Length < 1)
            return;

        Debug.Log("Found targets");
        
        var movementPosition = movement.CalculateTargetPosition(targets);
        movement.MoveTowards(movementPosition);

        var tar = attack.CalculateTarget(targets);
        if (tar == null)
            return;

        Debug.Log("Attacking Target");
        attack.Attack(tar);
    }
}