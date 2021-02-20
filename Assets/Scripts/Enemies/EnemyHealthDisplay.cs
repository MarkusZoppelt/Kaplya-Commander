using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthDisplay : InteractableDisplay
{
    [SerializeField] internal Destructable destructable;
    [SerializeField] internal Image healthDisplay;

    internal void Update()
    {
        healthDisplay.fillAmount = destructable.CurrentHealth / destructable.MaxHealth;
    }
}
