using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DeathEvent : UnityEvent<GameObject> { }

public class Destructable: MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private DeathEvent onDeath;
    private float currentHealth;

    internal void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        onDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
}