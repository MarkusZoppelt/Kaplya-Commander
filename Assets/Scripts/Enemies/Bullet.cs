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

    private void OnCollisionEnter(Collision collision)
    {
        var hitTarget = collision.collider.GetComponent<Destructable>();
        if (hitTarget)
            hitTarget.TakeDamage(damage);

        Destroy(gameObject);
    }
}
