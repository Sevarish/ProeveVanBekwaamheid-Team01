using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    private new Light light;

    public int framesLit = 60;

    private void Start()
    {
        light = GetComponent<Light>();

        light.intensity = 0;
    }
    void Update()
    {
        light.intensity++;

        if (light.intensity > framesLit)
        {
            light.gameObject.SetActive(false);
        }
    }
}
