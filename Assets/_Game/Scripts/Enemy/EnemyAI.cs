using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    patrolling = 0,
    checkpoints = 1,
};
public class EnemyAI : MonoBehaviour, Damageable
{
    //When this gameobject is selected you can use ctrl + 9 to add
    //a point with the current ground compared to scene view.
    //Game view cannot be used for this!

    public NavMeshAgent agent;  
    public Animator enemyAnim;

    [SerializeField]
    private float _sightDistance,
                  _attackRange,
                  _walkpointRange,
                  walkSpeed,
                  runSpeed;

    private bool playerInsideRange,
                 playerInsideAttackRange,
                 alreadyAttacked,
                 walkpointSet,
                 walkpointFlashed;

    [SerializeField]
    private Transform player;
    private Vector3 delayedPlayerPos;
    public float delayPosTime;

    [SerializeField]
    private LayerMask whatIsPlayer,
                      whatIsGround;

    [HideInInspector]
    public EnemyState enemyState = EnemyState.patrolling;


    private Vector3 walkpoint;

    [HideInInspector]
    public bool foundPlayer,
                alertedPatrolling;

    private int health = 100, damage = 25;

    public List<Vector3> points = new List<Vector3>();
    private int destPoint = 0;

    public Action EnemyDeath;
    private EnemyFov Fov;
    private AssaultRifle attacking;

    public AudioClip[] hurtSfx;
    public AudioClip[] deathSfx;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        Fov = GetComponent<EnemyFov>();
        attacking = GetComponent<AssaultRifle>();
        //enemyAnim = GetComponent<Animator>();
        StartCoroutine(GetDelayedPos());
        StartCoroutine(FlashedEffect());
    }

    private IEnumerator GetDelayedPos()
    {
        while (true)
        {
            delayedPlayerPos = player.transform.position;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator FlashedEffect()
    {
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(2f);
    }

    public void Flashed()
    {
        if (!walkpointFlashed)
        {
            float randomX = UnityEngine.Random.Range(-2, 2);
            float randomZ = UnityEngine.Random.Range(-2, 2);
            walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround)) walkpointFlashed = true;

            FlashedBehaviour();
        }
    }

    private void FlashedBehaviour()
    {
        if (walkpointFlashed)
        {
            print("test");
            agent.SetDestination(walkpoint);

            Vector3 distanceToWalkPoint = transform.position - walkpoint;

            if (distanceToWalkPoint.magnitude < 1f) { walkpointFlashed = false; }
        }

        if (alertedPatrolling == false)
        {
            alertedPatrolling = true;
        }
    }

    public void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.patrolling:
                playerInsideRange = Physics.CheckSphere(transform.position, _sightDistance, whatIsPlayer);
                playerInsideAttackRange = Physics.CheckSphere(transform.position, _attackRange, whatIsPlayer);
                if (alertedPatrolling) Patroling();
                if (!playerInsideRange && !foundPlayer) Fov.isInFov = false;
                if (Fov.isInFov) ChasePlayer();
                if (agent.velocity.magnitude < 0.15f) walkpointSet = false;
                break;

            case EnemyState.checkpoints:
                Checkpoints();
                break;
            default:
                break;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !alertedPatrolling)
            Checkpoints();
    }

    public void Checkpoints()
    {
        enemyAnim.SetBool("isShooting", false);

        // Returns if no points have been set up
        if (points.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint];

        // Choose the next point in the array as the destination
        destPoint = (destPoint + 1) % points.Count;
    }

    public void Patroling()
    {
        agent.speed = walkSpeed;

        //enemyAnim.Play("Patroling");
        enemyAnim.SetBool("isShooting", false);

        if (!walkpointSet) SearchWalkPoint();
        if (walkpointSet) agent.SetDestination(walkpoint);

        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        // walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) walkpointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomX = UnityEngine.Random.Range(-_walkpointRange, _walkpointRange);
        float randomZ = UnityEngine.Random.Range(-_walkpointRange, _walkpointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround)) walkpointSet = true;
    }

    public void ChasePlayer()
    {
        foundPlayer = true;
        if (playerInsideRange) Fov.isInFov = true;
        else { foundPlayer = false; Fov.isInFov = false; }
        agent.speed = runSpeed;

        agent.SetDestination(player.position);

        if (playerInsideAttackRange)
        {
            Attacking();
        }
        
    }

    public void Attacking()
    {
        enemyAnim.SetBool("isShooting", true);
        agent.SetDestination(transform.position);
        transform.LookAt(delayedPlayerPos);

         float timeBetweenAttacks = 1f;
         if (!alreadyAttacked)
         {
             attacking.Shoot();

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
        enemyAnim.enabled = false;
        Fov.enabled = false;
        enabled = false;
        //Fov.dropBody = true;
        //EnemyDeath?.Invoke();
    }

    void Damageable.TakeDamage(int _amount)
    {
        agent.SetDestination(player.transform.position);

        health -= _amount;
        Debug.Log(_amount);
        if (health <= 0)
        {
            Debug.Log("Death");
            SoundManager.Instance.RandomSoundEffect(deathSfx);
            Invoke(nameof(DestroyEnemy), 0.5f);
        } else
        {
            SoundManager.Instance.RandomSoundEffect(hurtSfx);
        }
    }
}

