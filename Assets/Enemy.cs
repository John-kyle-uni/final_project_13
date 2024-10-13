using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        // Reduce health by the damage amount
        currentHealth -= damage;

        // If health is 0 or less, call Die()
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Destroy the enemy object
        Destroy(gameObject);
    }
}
