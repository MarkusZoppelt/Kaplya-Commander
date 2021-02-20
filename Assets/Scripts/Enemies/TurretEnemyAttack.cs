using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyAttack : BaseEnemyAttack
{
    [SerializeField] internal Bullet bulletPrefab;
    [SerializeField] internal Transform muzzle;
    [SerializeField] internal float bulletSpeed;

    new public void Update()
    {
        base.Update();
    }

    public override void Attack(Destructable target)
    {
        if (coolDownTime > 0)
            return;


        audio?.Play();
        Bullet bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

        bullet.damage = this.damage;
        bullet.speed = bulletSpeed;
        bullet.direction = (target.transform.position - muzzle.position).normalized;
        bullet.direction.y = 0;
        coolDownTime = this.timeBetweenAttacks;
    }

    public override Destructable CalculateTarget(GameObject[] targets) {
        return base.CalculateTarget(targets);
    }
}