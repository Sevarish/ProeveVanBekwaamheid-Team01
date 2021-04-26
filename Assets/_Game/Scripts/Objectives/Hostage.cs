using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hostage : MonoBehaviour, Interactable
{
    public Action EnteredInteractRange;
    public Action OutOfRange;
    public Action SavedHostage;
    public float releaseTime = 6;
    public float interactRange = 5;
    public Slider slider;
    float oldReleaseTime = 0;
    private bool isSaving = false;

    private Transform currentInteractObject;

    public void Interact(Transform objectThatInteracted)
    {
        EnteredInteractRange?.Invoke();
        currentInteractObject = objectThatInteracted;
        if (!isSaving)
        {
            StartCoroutine("StartFreeingHostage");
        }
    }

    private void Start()
    {
        slider.maxValue = releaseTime;
        oldReleaseTime = releaseTime;
        slider.transform.gameObject.SetActive(false);
    }

    private void Update()
    {
        slider.value = releaseTime;
    }

    private IEnumerator StartFreeingHostage()
    {
        isSaving = true;
        while (currentInteractObject != null)
        {
            slider.transform.gameObject.SetActive(true);
            if (Vector3.Distance(transform.position, currentInteractObject.position) > interactRange)
            {
                releaseTime = oldReleaseTime;
                OutOfRange?.Invoke();
                slider.transform.gameObject.SetActive(false);
                currentInteractObject = null;
                isSaving = false;
            }
            if(releaseTime <= 0)
            {
                currentInteractObject = null;
                SavedHostage?.Invoke();
                Debug.Log("saved");
                slider.transform.gameObject.SetActive(false);
            }
            releaseTime -= 0.10f;
            yield return new WaitForSeconds(0.10f);
        }
    }
}
