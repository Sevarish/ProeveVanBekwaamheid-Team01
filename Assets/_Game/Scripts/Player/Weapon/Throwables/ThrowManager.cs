using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    public KeyCode throwButton;
    public List<GameObject> grenades;
    private int currentGrenade = 0;
    public int startAmountGrenades = 2;
    private int grenadeAmmo;
    public float throwForce;

    private void Start()
    {
        grenadeAmmo = startAmountGrenades;
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwButton))
        {
            ThrowGrenade();
        }
    }

    public void ThrowGrenade()
    {
        if(grenadeAmmo > 0)
        {
            grenadeAmmo--;
            GameObject thrownGrenade = Instantiate(grenades[currentGrenade], transform.position + (transform.forward * 2), transform.rotation);
            thrownGrenade.GetComponent<Grenade>().Throw(transform.forward, throwForce);
        }
    }
}
