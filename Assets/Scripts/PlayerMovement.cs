using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Transform targetEnemy;

    public Camera cam;
    public float attackRange = 2f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))    //Right click to move
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    targetEnemy = hit.collider.transform;

                    agent.SetDestination(targetEnemy.position);
                }
                else
                {
                    targetEnemy = null;

                    agent.SetDestination(hit.point);
                }
            }
        }

        if (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.position) <= attackRange)
        {
            AttackEnemy();
        }

        //Setting animation based on movement
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void AttackEnemy()
    {
        agent.ResetPath();
        transform.LookAt(targetEnemy);
        animator.SetTrigger("Attack");

        EnemyHealth enemyHealth = targetEnemy.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(50); //Deals 50 damage
        }
    }
}
