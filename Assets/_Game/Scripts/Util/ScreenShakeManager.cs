using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    private Transform player;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    public void Shake(float duration)
    {
        shakeDuration = duration;
    }

    public void Shake(float duration, float newshakeAmount)
    {
        shakeAmount = newshakeAmount;
        shakeDuration = duration;
    }

    public void Shake(float duration, float newshakeAmount, float newDecreaseFactor)
    {
        decreaseFactor = newDecreaseFactor;
        shakeAmount = newshakeAmount;
        shakeDuration = duration;
    }

    void Awake()
    {
        player = GameObject.FindObjectOfType<Player>().transform;
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
        }
    }
}
