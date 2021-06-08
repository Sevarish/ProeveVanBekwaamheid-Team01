using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowManager : MonoBehaviour
{
    public KeyCode throwButton;
    public List<GameObject> grenades;
    private int currentGrenade = 0;
    public int startAmountGrenades = 2;
    private int grenadeAmmo;
    public float throwForce;
    public List<Image> grenadeSprites = new List<Image>();

    private void Start()
    {
        grenadeAmmo = startAmountGrenades;
    }

    private void SetSpritesActive()
    {
        for(int i = 0; i < startAmountGrenades; i++)
        {
            if(i > grenadeAmmo-1)
            {
                grenadeSprites[i].gameObject.SetActive(false);
            }
            else
            {
                grenadeSprites[i].gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        SetSpritesActive();
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
