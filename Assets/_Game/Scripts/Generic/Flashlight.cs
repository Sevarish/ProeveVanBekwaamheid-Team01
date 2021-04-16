using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private KeyCode flashLightKey = KeyCode.E;
    [SerializeField]
    private Transform crossHair;

    private bool isTurnedOn = true;
    private new Light light;
    [SerializeField]
    private int secondsBeforeStartingRefill;
    [SerializeField]
    private float batteryTimeLeft;
    [SerializeField]
    private float maxBatteryLife;

    private bool isRecharging = false;

    private void Start()
    {
        light = GetComponent<Light>();

        batteryTimeLeft = maxBatteryLife;
    }

    private void Update()
    {
        FollowMouse();

        if (Input.GetKeyDown(flashLightKey) && batteryTimeLeft > 0)
        {
            ToggleFlashLight();
        }
        if (isTurnedOn && batteryTimeLeft > 0)
        {
            UpdateBatteryLife();
        }
        if (Input.GetKeyDown(flashLightKey)) 
        {
            isRecharging = false;
        }
    }

    private void UpdateBatteryLife()
    {
        batteryTimeLeft -= Time.deltaTime;
        if (batteryTimeLeft <= 0)
        {
            // when battery is empty
            isTurnedOn = false;
            light.enabled = false;
            batteryTimeLeft = 0;
            StartCoroutine(WaitForRefill(secondsBeforeStartingRefill));
        }
    }

    private IEnumerator WaitForRefill(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        isRecharging = true;
        StartCoroutine(Refilling(0.25f));
    }
    private IEnumerator Refilling(float timeInBetweenTicks)
    {
        while(isRecharging)
        {
            yield return new WaitForSeconds(timeInBetweenTicks);

            RefillBattery(1);
            if (batteryTimeLeft >= maxBatteryLife)
            {
                StopAllCoroutines();
            }
        }
    }

    private void RefillBattery(int amount)
    {
        batteryTimeLeft += amount;
        if (batteryTimeLeft > maxBatteryLife)
        {
            batteryTimeLeft = maxBatteryLife;
        }
    }

    private void ToggleFlashLight()
    {
        isTurnedOn = !isTurnedOn;

        if (isTurnedOn)
        {
            light.enabled = true;
            StopAllCoroutines();
        }
            
        if (!isTurnedOn)
        {
            light.enabled = false;
            StartCoroutine(WaitForRefill(secondsBeforeStartingRefill));
        }
    }

    private void FollowMouse()
    {
        //Rotate the visual object of the player towards the targetting point.
        Vector3 tarDir = crossHair.transform.position - light.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(light.transform.forward, tarDir, 1, 0.0f);
        light.transform.rotation = Quaternion.LookRotation(newDirection);
        light.transform.localEulerAngles = new Vector3(0, light.transform.localEulerAngles.y, 0);
    }
}
