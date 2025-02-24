using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Vector3 wanderTarget;
    private Animator animator;
    private PlayerHealth playerHealth;

    public float detectionRange = 7f;
    public float wanderRadius = 10f;
    private bool isChasing = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();

        SetRandomWanderTarget();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            isChasing = true;

            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            isChasing = false;

            SetRandomWanderTarget();
        }

        if (!isChasing && agent.remainingDistance < 0.5f)
        {
            SetRandomWanderTarget();
        }

        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void SetRandomWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

        randomDirection += transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;

            agent.SetDestination(wanderTarget);
        }
    }

    private void AttackPlayer()
    {
        agent.ResetPath();
        transform.LookAt(player);
        animator.SetTrigger("Attack");

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(20);    //Enemy deals 20 damage
        }
    }
}
