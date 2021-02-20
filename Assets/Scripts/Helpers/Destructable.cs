using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DeathEvent : UnityEvent<GameObject> { }

public class Destructable: MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] public DeathEvent onDeath;
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get { return maxHealth; } }

    internal void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if(CurrentHealth <= 0f)
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