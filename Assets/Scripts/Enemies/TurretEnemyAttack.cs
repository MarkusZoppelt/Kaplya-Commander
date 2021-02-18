using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyAttack : BaseEnemyAttack
{
    [SerializeField] internal Bullet bulletPrefab;
    [SerializeField] internal Transform muzzle;
    [SerializeField] internal float bulletSpeed;

    public override void Attack(Destructable target)
    {
        Debug.Log("Attacking... spawning bullet");
        Bullet bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

        bullet.damage = this.damage;
        bullet.speed = bulletSpeed;
        bullet.direction = (target.transform.position - muzzle.position).normalized;
        bullet.direction.y = 0;
    }

    public override Destructable CalculateTarget(GameObject[] targets) {
        return base.CalculateTarget(targets);
    }
}