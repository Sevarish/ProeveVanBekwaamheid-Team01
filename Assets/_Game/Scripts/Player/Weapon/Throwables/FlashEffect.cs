using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    private new Light light;

    public float secondsToFlash = 1;

    private void Start()
    {
        light = GetComponent<Light>();
        secondsToFlash *= 60;

        light.intensity = 100;
    }
    void Update()
    {
        light.intensity--;

        if (light.intensity < secondsToFlash)
        {
            Destroy(this.gameObject);
        }
    }
}
