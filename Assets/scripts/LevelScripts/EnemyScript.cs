using UnityEngine;
using UnityEngine.AI;  // Required for NavMesh
using System.Collections; // Add this line


public class EnemyScript : MonoBehaviour
{
    public enum FSMState
    {
        Attack,
        Dead,
    }

    public FSMState curState;
    public Transform player;         // Reference to the player's position
    private NavMeshAgent agent;      // Reference to the NavMeshAgent component
    public int enemyHealth;

    public GameObject Coin;
    
    // Attack variables
    public int EnemyDamage = 20 * DataHolder.Difficulty;          // Damage dealt by the enemy
    private bool canAttack = true;   // To track if the enemy can attack
    public float attackCooldown = 2f; // Cooldown duration in seconds
    
    void Start()
    {
        curState = FSMState.Attack;
        // Get the NavMeshAgent component attached to the enemy
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = 100 * DataHolder.Difficulty;
    }

    void Update()
    {
        switch (curState)
        {
            case FSMState.Attack: UpdateAttack(); break;
            case FSMState.Dead: UpdateDead(); break;
        }
    }

    protected void UpdateAttack()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        
        agent.SetDestination(player.position);

        // Check if the enemy can attack
        if (distanceToPlayer < 2f && canAttack) // Adjust the distance as needed
        {
            AttackPlayer();  // Call the attack function
        }

        if (enemyHealth <= 0)
        {
            curState = FSMState.Dead;
        }
    }

    private void AttackPlayer()
    {
        // Assume player has a HealthManager component to apply damage
        player.GetComponent<HealthManager>().ApplyDamage(EnemyDamage);
        Debug.Log("Enemy attacked the player!");

        // Start the cooldown coroutine
        StartCoroutine(AttackCooldown());
    }

    protected void UpdateDead()
    {
        Debug.Log("Enemy is dead, destroying GameObject.");
        Instantiate(Coin, transform.position, transform.rotation);
        Destroy(gameObject);
    }


    public void TakeHit(int damage)
    {
        Debug.Log(damage);
        enemyHealth -= damage;

        // Optionally, check if health is below or equal to zero after taking damage
        if (enemyHealth <= 0)
        {
            curState = FSMState.Dead;  // Transition to dead state if health is zero
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false; // Prevent further attacks
        yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown duration
        canAttack = true; // Allow attacks again
    }
    
}
