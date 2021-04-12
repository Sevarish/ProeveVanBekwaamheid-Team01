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

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        FollowMouse();

        if (Input.GetKeyDown(flashLightKey))
        {
            ToggleFlashLight();
        }
    }

    private void ToggleFlashLight()
    {
        isTurnedOn = !isTurnedOn;

        if (isTurnedOn)
            light.enabled = true;
        if (!isTurnedOn)
            light.enabled = false;
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
