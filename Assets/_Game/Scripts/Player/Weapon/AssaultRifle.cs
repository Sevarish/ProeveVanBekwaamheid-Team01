using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{

    public GameObject muzzleFlashPrefab;
    public GameObject sourceEmitter;
    public string targetTag;

    private readonly float fireRate = 0.12f;

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
}
