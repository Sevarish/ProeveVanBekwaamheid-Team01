using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int lifes;
    public float speed = 8.5f,
                 vision,
                 flashBatteryLife;
    public AssaultRifle HK416;
    public Taser X26;
    public bool Died;
    private GameObject visionCone;
    public int currentWeapon; //0 is HK416, 1 is Taser

    void Start()
    {

    }

    public void TakeDamage(int _amount)
    {
        lifes -= _amount;
    }

    private void SetVision()
    {

    }

    public void SwitchWeapon()
    {
        if (currentWeapon == 0) { currentWeapon = 1; Debug.Log(currentWeapon); return; }
        if (currentWeapon == 1) { currentWeapon = 0; Debug.Log(currentWeapon); return; }
    }

    public void ToggleFlashLight()
    {

    }

    private IEnumerator RechargeFlash()
    {
        yield return new WaitForSeconds(0.1f);
    }

    
}
