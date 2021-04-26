using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    patrolling = 0,
    checkpoints = 1,
};
public class EnemyAI : MonoBehaviour, Damageable
{
    [SerializeField]
    private NavMeshAgent agent;

    //[SerializeField]
    //private Animator enemyAnim;

    [SerializeField]
    private float _sightDistance,
                  _attackRange,
                  _walkpointRange,
                  walkSpeed,
                  runSpeed;

    [SerializeField]
    private bool playerInsideRange,
                 playerInsideAttackRange,
                 alreadyAttacked;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private LayerMask whatIsPlayer,
                     whatIsGround;

    public EnemyState enemyState = EnemyState.patrolling;

    private bool walkpointSet;

    private Vector3 walkpoint;

    public bool foundPlayer,
                foundBody;

    // already added for the damaging/death system
    private int health = 100, damage = 10;

    public Vector3[] points;
    private int destPoint = 0;

    public EnemyFov Fov;
    public AssaultRifle attacking;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        Fov = GetComponent<EnemyFov>();
        attacking = GetComponent<AssaultRifle>();
        //enemyAnim = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.patrolling:
                playerInsideRange = Physics.CheckSphere(transform.position, _sightDistance, whatIsPlayer);
                playerInsideAttackRange = Physics.CheckSphere(transform.position, _attackRange, whatIsPlayer);
                //if (!EnemyFov.isInFov) {  }
                if (foundBody) Patroling();
                if (!playerInsideRange && !foundPlayer) Fov.isInFov = false;
                if (Fov.isInFov) ChasePlayer();
                if (agent.velocity.magnitude < 0.15f) walkpointSet = false;
                break;
            // TO DO:
            // set the checkpoint function as a state

            case EnemyState.checkpoints:
                break;
                // NOTE:
                // deleted the rotating and finished patrolling state
                // had both to prevent that the enemy gets stuck on a corner.
                // now when his speed is under a certain value he will choose a new path.
            default:
                break;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !foundBody)
            Checkpoints();
    }

    public void Checkpoints()
    {
        // TO DO:
        // Add Walking animation
        //enemyAnim.Play("Walking");

        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint];

        // Choose the next point in the array as the destination
        destPoint = (destPoint + 1) % points.Length;
    }

    public void Patroling()
    {
        agent.speed = walkSpeed;

        // TO DO:
        // Add patrolling animation
        //enemyAnim.Play("Patroling");


        if (!walkpointSet) SearchWalkPoint();
        if (walkpointSet) agent.SetDestination(walkpoint);

        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        // walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) walkpointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-_walkpointRange, _walkpointRange);
        float randomZ = Random.Range(-_walkpointRange, _walkpointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround)) walkpointSet = true;
    }

    public void ChasePlayer()
    {
        if (!foundPlayer)
        {
            //SoundManager.PlaySound("Shout");
        }
        foundPlayer = true;
        if (playerInsideRange) Fov.isInFov = true;
        else { foundPlayer = false; Fov.isInFov = false; }
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        // enemyAnim.Play("Running");


        //TO DO: 
        //set the variable for the playerInsideAttackRange in the editor
        if (playerInsideAttackRange)
        {
            Attacking();
        }

    }

    public void Attacking()
    {
        float timeBetweenAttacks = 1f;
        //TO DO: 
        //Let the enemy shoot with a weapon towards the player
        if (!alreadyAttacked)
        {
            attacking.Shoot();
            Debug.Log("Enemy is shooting");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
        // add here the part where the death body will be dropped at the death of the enemy
    }

    void Damageable.TakeDamage()
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
}

