using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        Destructable destructable = other.GetComponent<Destructable>();
        if (destructable == null)
        {
            return;
        }

        destructable.TakeDamage(damage);
    }
}
