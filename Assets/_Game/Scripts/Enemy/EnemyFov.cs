using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFov : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject[] enemies;

    [Range(0, 180)]
    [SerializeField]
    private float maxAngle, alertAngle;

    [Range(0, 20)]
    [SerializeField]
    private float maxRadius, alertRadius;

    [SerializeField]
    private float foundPlayerRadius,
                  foundPlayerAngle,
                  originalRadius,
                  originalAngle,
                  heightMultiplayer;

    [HideInInspector]
    public bool isInFov = false,
                dropBody;

    private EnemyAI AI;

    [SerializeField]
    private GameObject body;

    private bool chasingEnemy;

    private bool inFOV = false,
                 inAlertFOV = false;

    private GameObject[] deadBody;

    public void Awake()
    {
        AI = GetComponent<EnemyAI>();
        AI.EnemyDeath += DropBody;
    }
    public void OnDrawGizmos()
    {
        if (isInFov && AI.foundPlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, foundPlayerRadius);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, alertRadius);
        }

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;
        
        Vector3 fovLine3 = Quaternion.AngleAxis(alertAngle, transform.up) * -transform.forward * alertRadius;
        Vector3 fovLine4 = Quaternion.AngleAxis(-alertAngle, transform.up) * -transform.forward * alertRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.gray;
        Gizmos.DrawRay(transform.position, fovLine3);
        Gizmos.DrawRay(transform.position, fovLine4);

        Gizmos.color = Color.red;
        if (isInFov)
            Gizmos.color = Color.green;
        if (!isInFov)
            Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, -transform.forward * alertRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, transform.forward * 3.5f);
    }

    //check if the player is in the Fov
    public void InFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius, float alertRadius)
    {
        deadBody = GameObject.FindGameObjectsWithTag("DeadEnemy");
        Vector3 directionBetween = (target.position - checkingObject.position).normalized;
        directionBetween.y *= 0;

        RaycastHit hit;

        //Max radius frontal vision AI
        if (Physics.Raycast(checkingObject.position + Vector3.up * heightMultiplayer, (target.position - checkingObject.position).normalized, out hit, maxRadius))
        {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
            {
                float angle = Vector3.Angle(checkingObject.forward + Vector3.up * heightMultiplayer, directionBetween);
                float outerAngle = maxAngle + 2;
                if (angle <= maxAngle)
                {
                    FollowPlayer();
                    inFOV = true;
                }
                else if (!inAlertFOV) isInFov = false;
            }
        } else gameObject.layer = 0;

        //Alert radius back vision AI
        if (Physics.Raycast(checkingObject.position + Vector3.up * heightMultiplayer, (target.position - checkingObject.position).normalized, out hit, alertRadius))
        {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
            {
                float angle = Vector3.Angle(checkingObject.forward + Vector3.up * heightMultiplayer, -directionBetween);
                if (angle <= alertAngle)
                {
                    FollowPlayer();
                    inAlertFOV = true;
                }
                else if (!inFOV) isInFov = false;
            }
        }
        {
            Vector3 dirBetweenTest = (deadBody[i].transform.position - checkingObject.position).normalized;
            dirBetweenTest.y *= 0;
            
            if (Physics.Raycast(checkingObject.position + Vector3.up * heightMultiplayer, (deadBody[i].transform.position - checkingObject.position).normalized, out hit, maxRadius))
            {
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "DeadEnemy")
                {
                    float angle = Vector3.Angle(checkingObject.forward + Vector3.up * heightMultiplayer, dirBetweenTest);
                    if (angle <= maxAngle)
                    {
                        AI.alertedPatrolling = true;
                    }
                }
            }
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dirBetween = (enemies[i].transform.position - checkingObject.position).normalized;
            dirBetween.y *= 0;

            if (Physics.Raycast(checkingObject.position + Vector3.up * heightMultiplayer, (enemies[i].transform.position - checkingObject.position).normalized, out hit, maxRadius))
            {
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "ChasingEnemy")
                {
                    float angle = Vector3.Angle(checkingObject.forward + Vector3.up * heightMultiplayer, dirBetween);
                    if (angle <= maxAngle)
                    {
                        AI.agent.SetDestination(enemies[i].transform.position);
                        chasingEnemy = true;
                    }
                }
            } else chasingEnemy = false; 
        }
    }

    private void FollowPlayer()
    {
        isInFov = true;
        gameObject.layer = 9;
        AI.foundPlayer = true;
        if (!chasingEnemy)
        {
            AI.alertedPatrolling = true;
        }
    }

    public void DropBody()
    {
        if (dropBody)
        {
            Instantiate(body, transform.position, Quaternion.identity);
        }
    }

    public void FixedUpdate()
    {
        InFOV(transform, player, maxAngle, maxRadius, alertRadius);
        if (isInFov && AI.foundPlayer || chasingEnemy)
        {
            maxAngle = foundPlayerAngle;
            maxRadius = foundPlayerRadius;
        }
        else
        {
            maxRadius = originalRadius;
            maxAngle = originalAngle;
        }
    }
}