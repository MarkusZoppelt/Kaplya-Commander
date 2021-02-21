using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricField : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        var blob = other.GetComponent<ConductorBlob>();
        if (blob != null)
        {
            return;
        }

        Destructable destructable = other.GetComponent<Destructable>();
        if(destructable == null)
        {
            return;
        }

        destructable.TakeDamage(damage);
    }
}
