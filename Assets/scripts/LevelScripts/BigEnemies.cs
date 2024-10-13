using UnityEngine;
using UnityEngine.AI;  // Required for NavMesh
using System.Collections; // Add this line


public class BigEnemies : MonoBehaviour
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
    public int damage = 10;          // Damage dealt by the enemy
    private bool canAttack = true;   // To track if the enemy can attack
    public float attackCooldown = 2f; // Cooldown duration in seconds

    public float attackRange;

    // Death Spawn
    public GameObject newEnemies;
    public Transform spawnLocation;
    
    void Start()
    {
        curState = FSMState.Attack;
        // Get the NavMeshAgent component attached to the enemy
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = 100;
        

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if(Coin == null)
        {
            Coin = GameObject.FindGameObjectWithTag("coins");
        }
        if(spawnLocation == null)
        {
            spawnLocation = GameObject.FindGameObjectWithTag("spawnpoint").transform;
        }
        placeNavEnemy();
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
        
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(player.position);  // Move the enemy towards the player
        }
        else
        {
            Debug.LogError("NavMeshAgent is not active or missing!");
        }
        // Check if the enemy can attack
        if (distanceToPlayer < attackRange && canAttack) // Adjust the distance as needed
        {
            AttackPlayer();  // Call the attack function
        }

        if (enemyHealth <= 0)
        {
            curState = FSMState.Dead;
        }
    }
    //  protected void UpdateAttack()
    // {
    //     // Calculate the distance between the enemy and the player
    //     float distanceToPlayer = Vector3.Distance(player.position, transform.position);

    //     // Ensure the NavMeshAgent is active
    //     if (agent != null && agent.isActiveAndEnabled)
    //     {
    //         agent.SetDestination(player.position);  // Move the enemy towards the player
    //     }
    //     else
    //     {
    //         Debug.LogError("NavMeshAgent is not active or missing!");
    //     }

    //     // Check if the enemy can attack
    //     if (distanceToPlayer < attackRange && canAttack)
    //     {
    //         AttackPlayer();
    //     }

    //     if (enemyHealth <= 0)
    //     {
    //         curState = FSMState.Dead;
    //     }
    // }
    private void AttackPlayer()
    {   
        InitNavMesh();
        setPlayer(player);
        // Assume player has a HealthManager component to apply damage
        player.GetComponent<HealthManager>().ApplyDamage(damage);
        Debug.Log("Enemy attacked the player!");

        // Start the cooldown coroutine
        StartCoroutine(AttackCooldown());
    }

    protected void UpdateDead()
    {
        Debug.Log("Enemy is dead, destroying GameObject.");
        Instantiate(Coin, transform.position, transform.rotation);
        Destroy(gameObject);

        FindObjectOfType<SpawnManager>().EnemyDied();
    }


    public void TakeHit(int damage)
    {
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
    public void InitNavMesh()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found!");
        }
        else
        {
            agent.enabled = true;
        }
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
    public void setPlayer(Transform playerTransform)
    {
        player = playerTransform;
        InitNavMesh(); 
    }
    
    void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    void placeNavEnemy()
    {
        if (agent != null)
        {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
        {
            transform.position = hit.position; 
            agent.enabled = true;  
            agent.SetDestination(player.position);  
        }
        else
        {
            Debug.LogError("Failed to find a valid position on the NavMesh!");
            transform.position = new Vector3(0, 0, 0);  
            agent.SetDestination(player.position);  
        }
    }
    else
        {
            Debug.LogError("NavMeshAgent component missing!");
        }
    }
}
