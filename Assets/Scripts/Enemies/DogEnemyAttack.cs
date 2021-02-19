using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemyAttack : BaseEnemyAttack
{
    [SerializeField] internal float byteTimer;

    public override void Attack(Destructable target)
    {
        // hook onto target and attack until byteTimer runs out
        base.Attack(target);
    }
}
