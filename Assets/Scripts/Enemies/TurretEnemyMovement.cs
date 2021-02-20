using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyMovement : BaseEnemyMovement
{
    [SerializeField] internal Transform turretHead;

    public override void MoveTowards(Vector3 targetPosition)
    {
        targetPosition.y = turretHead.position.y;
        turretHead.LookAt(targetPosition);
    }
}
