using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetect : MonoBehaviour
{
    public SwordControl sc;
    public GameObject HitParticle;

    public int damage = 50;
    private Vector3 newPos;
    // Trigger detection when the sword collides with an enemy
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy") && sc.isAttacking)  // Check if the sword is attacking
        {
            Debug.Log("Enemy hit: " + other.name);
            
            // Get the EnemyScript component from the enemy GameObject
            EnemyScript enemyScript = other.GetComponent<EnemyScript>();
			BigEnemies enemyScript1 = other.GetComponent<BigEnemies>()	;
            if (enemyScript != null) // Check if the script was found
            {
                // Apply damage to the enemy
                enemyScript.TakeHit(damage);
            }
            
					
            else if (enemyScript1 != null) // Check if the script was found
            {
                // Apply damage to the enemy
                enemyScript1.TakeHit(damage);
            }
            else
            {
                Debug.LogWarning("No EnemyScript found on " + other.name);
            }
        }
    }
}
