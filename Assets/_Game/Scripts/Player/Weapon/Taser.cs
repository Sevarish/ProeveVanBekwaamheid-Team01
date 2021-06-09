using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taser : MonoBehaviour
{
    [SerializeField]
    GameObject targetPoint, //The object which the player will always rotate towards. AKA targetting point. (Crosshair)
               playerEmitter; //The emit point for bullets and the taser projectiles.

    private float FireRate = 10f;
    public int clipCapacity = 3;
    public void Shoot()
    {
        //Casts a raycast from the playerEmitter towards the crosshair (targetPoint).
        RaycastHit hit;
        Vector3 cursorPos = GameObject.FindObjectOfType<CursorController>().transform.position;
        Vector3 direction = cursorPos - playerEmitter.transform.position;
        Debug.DrawRay(playerEmitter.transform.position, direction * 20, Color.yellow, 1);
        if (Physics.Raycast(playerEmitter.transform.position, direction  * 20, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<Damageable>().TakeDamage(1000);
            }
        }
    }

    public float GetFireRate()
    {
        return FireRate;
    }
}
