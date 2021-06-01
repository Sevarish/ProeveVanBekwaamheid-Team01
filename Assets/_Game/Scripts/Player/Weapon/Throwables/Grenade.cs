using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrenadeType { FlashGrenade, NormalGrenade }

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Grenade : MonoBehaviour
{
    [Tooltip("Time before it explodes")]
    public float fuseTime = 2f;
    [Tooltip("Defines how the grenade interacts")]
    public GrenadeType thisGrenadeType;
    [Tooltip("Explosion Radius")]
    public float explosionRadius = 3f;
    [Tooltip("Force applied if applyPhysicsToRigidbodys = true")]
    public float explosionForce = 10f;
    [Tooltip("Starts explosion when it spawns or is active")]
    public bool fuseOnAwake = false;
    [Tooltip("Starts explosion if it hits a collider")]
    public bool fuseOnCollision = false;
    [Tooltip("Force applied to other objects yes/no")]
    public bool applyPhysicsToRigidbodys = false;
    private Rigidbody applyPhysics;

    private void Awake()
    {
        applyPhysics = gameObject.GetComponent<Rigidbody>();
        if (fuseOnAwake)
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (fuseOnCollision)
        {
            Explode();
        }
    }

    public void Explode()
    {
        StartCoroutine(ExplodeTimer());
    }

    private IEnumerator ExplodeTimer()
    {
        yield return new WaitForSeconds(fuseTime);
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRadius, new Vector3(0,1,0));
        foreach (RaycastHit hit in hits)
        {
            //Do Damage
            if(thisGrenadeType == GrenadeType.NormalGrenade)
            {
                hit.transform.GetComponent<Damageable>()?.TakeDamage();
            }
            if (thisGrenadeType == GrenadeType.FlashGrenade)
            {
                //FLASH BEHAVIOUR ON ENEMY
                hit.transform.GetComponent<EnemyAI>()?.Flashed();
            }
            //Apply physics
            if (applyPhysicsToRigidbodys && hit.transform.GetComponent<Rigidbody>() != null)
            {
                hit.transform.GetComponent<Rigidbody>()?.AddForce((hit.transform.position - transform.position) * explosionForce, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }

    public void Throw(Vector3 direction)
    {
        applyPhysics.AddForce(direction * 2, ForceMode.Impulse);
    }

    public void Throw(Vector3 direction, float force)
    {
        applyPhysics.AddForce(direction * force, ForceMode.Impulse);
    }

}
