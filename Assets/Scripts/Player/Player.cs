using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int lifes;
    public float speed = 8.5f,
                 vision,
                 flashBatteryLife;
    public List<BaseWeapon> weapons = new List<BaseWeapon>();
    public bool Died;
    private GameObject visionCone;
    private int currentWeapon;

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

    }

    public void SwitchWeapon(BaseWeapon weapon)
    {

    }

    public BaseWeapon GetCurrentWeapon()
    {
        return weapons[currentWeapon];
    }

    public void ToggleFlashLight()
    {

    }

    private IEnumerator RechargeFlash()
    {
        yield return new WaitForSeconds(0.1f);
    }

    
}
