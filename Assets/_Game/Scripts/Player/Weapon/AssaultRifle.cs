using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    [SerializeField]
    GameObject targetPoint, //The object which the player will always rotate towards. AKA targetting point. (Crosshair)
               playerEmitter, //The emit point for bullets and the taser projectiles.
               MF;
        
    private float FireRate = 0.12f;

    public void Shoot()
    {
        Quaternion rotParent = playerEmitter.transform.parent.rotation;
        var MuzzleFlash = Instantiate(MF, playerEmitter.transform.position, rotParent);
        MuzzleFlash.transform.SetParent(playerEmitter.transform.parent);
        Destroy(MuzzleFlash, 0.2f);

        Debug.DrawRay(playerEmitter.transform.position, (targetPoint.transform.position - playerEmitter.transform.position) * 20, Color.black, 1);
        //Casts a raycast from the playerEmitter towards the crosshair (targetPoint).
        RaycastHit hit;
        if (Physics.Raycast(playerEmitter.transform.position, (targetPoint.transform.position - playerEmitter.transform.position) * 20, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Damageable>().TakeDamage();
            }
        }
    }

    public float GetFireRate()
    {
        return FireRate;
    }
}
