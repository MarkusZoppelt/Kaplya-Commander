using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float damage;
    [SerializeField] public float speed;
    [SerializeField] public Vector3 direction;

    public void Update()
    {
        transform.position += (Time.deltaTime * direction * speed);
    }

    public void OnTriggerEnter(Collider coll)
    {
        var hitTarget = coll.GetComponent<Destructable>();
        if (hitTarget)
            hitTarget.TakeDamage(damage);

        Destroy(gameObject);
    }
}
