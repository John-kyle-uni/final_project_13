using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetect : MonoBehaviour
{
    public SwordControl sc;         // Reference to the sword control script
    public int damage = 50;         // Damage dealt to enemies
    public HealthManager playerHealth; // Reference to player health (if needed)

    void Update()
    {
        // Update logic can be added here if necessary
    }

    // Trigger detection when the sword collides with an enemy
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy") && sc.isAttacking)  // Check if the sword is attacking
        {
            Debug.Log("Enemy hit: " + other.name);
            
            // Get the EnemyScript component from the enemy GameObject
            EnemyScript enemyScript = other.GetComponent<EnemyScript>();
            if (enemyScript != null) // Check if the script was found
            {
                // Apply damage to the enemy
                enemyScript.TakeHit(damage);
            }
            else
            {
                Debug.LogWarning("No EnemyScript found on " + other.name);
            }
        }
    }
}
