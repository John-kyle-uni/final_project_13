using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public enum FSMState
    {
        Patrol,
        Attack,
        Dead,
    }

    public FSMState curState;
    public Transform player;
    private NavMeshAgent agent;
    public int enemyHealth;
    public GameObject Coin;
    public int EnemyDamage = 20 * DataHolder.Difficulty;
    private bool canAttack = true;
    public float attackCooldown = 2f;
    
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        curState = FSMState.Patrol;
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = 100 * DataHolder.Difficulty;
        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrol(); break;
            case FSMState.Attack: UpdateAttack(); break;
            case FSMState.Dead: UpdateDead(); break;
        }
    }

    protected void UpdatePatrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer < 10f)
        {
            curState = FSMState.Attack;
        }

        if (enemyHealth <= 0)
        {
            curState = FSMState.Dead;
        }
    }

    protected void UpdateAttack()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        agent.SetDestination(player.position);

        if (distanceToPlayer < 2f && canAttack)
        {
            AttackPlayer();
        }

        if (enemyHealth <= 0)
        {
            curState = FSMState.Dead;
        }
    }

    private void AttackPlayer()
    {
        player.GetComponent<HealthManager>().ApplyDamage(EnemyDamage);
        StartCoroutine(AttackCooldown());
    }

    protected void UpdateDead()
    {
        Instantiate(Coin, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void TakeHit(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            curState = FSMState.Dead;
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
