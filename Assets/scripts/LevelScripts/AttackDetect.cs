using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetect : MonoBehaviour
{
    public SwordControl sc;
    public GameObject HitParticle;

    public int damage = 50;
    private Vector3 newPos;

    void Update()
	{

<<<<<<< Updated upstream
		// see if bullet hits a collider
		RaycastHit hit;
		if (Physics.Linecast(transform.position, newPos, out hit))
		{
			if (hit.collider)
			{




				
				// apply damage to object
				GameObject obj = hit.collider.gameObject;
				if ((obj.tag == "enemy" && sc.isAttacking))
					obj.SendMessage("ApplyDamage", damage);
			}
		}
	}
    // private void OnTrigger(Collider other){

    //     if(other.tag == "enemy" && sc.isAttacking)
    //     {
    //         Debug.Log(other.name);
    //         GameObject obj = hit.collider.gameObject;
	// 			if (obj.tag == "enemy")
	// 				obj.SendMessage("ApplyDamage", damage);
    //     }
    // }
=======
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
>>>>>>> Stashed changes
}
