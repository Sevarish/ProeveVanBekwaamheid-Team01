using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{

    public GameObject muzzleFlashPrefab;
    public GameObject bulletPrefab; //Visual only.
    public GameObject sourceEmitter;
    private ParticleSystem bParticle;
    public int clipCapacity = 32;
    public int fullCapacity = 64;
    public string targetTag;

    private readonly float fireRate = 0.12f;

    private void Start()
    {
        if (transform.tag == "Player")
        {
            Quaternion rotParent = sourceEmitter.transform.parent.rotation;
            var bullet = Instantiate(bulletPrefab, sourceEmitter.transform.position, rotParent);
            bullet.transform.SetParent(sourceEmitter.transform.parent);
            bParticle = bullet.GetComponentInChildren<ParticleSystem>();
            bParticle.Stop();
        }
    }

    public void Shoot()
    {
        Quaternion rotParent = sourceEmitter.transform.parent.rotation;
        var MuzzleFlash = Instantiate(muzzleFlashPrefab, sourceEmitter.transform.position, rotParent);
        MuzzleFlash.transform.SetParent(sourceEmitter.transform.parent);
        Destroy(MuzzleFlash, 0.2f);


        Debug.DrawRay(sourceEmitter.transform.position, sourceEmitter.transform.forward, Color.black, 1);
        //Casts a raycast from the playerEmitter towards the crosshair (targetPoint).
        RaycastHit hit;
        if (Physics.Raycast(sourceEmitter.transform.position, sourceEmitter.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag(targetTag))
            {
                hit.transform.gameObject.GetComponent<Damageable>().TakeDamage();
            }
        }
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public void StartPtcl()
    {
        bParticle.Play();
    }

    public void StopPtcl()
    {
        bParticle.Stop();
    }
}
