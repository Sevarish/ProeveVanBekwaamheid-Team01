using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Hostage : MonoBehaviour, Interactable
{
    public Action EnteredInteractRange;
    public Action OutOfRange;
    public Action SavedHostage;
    public float releaseTime = 6;
    public float interactRange = 5;
    public Image slider;
    float oldReleaseTime = 0;
    private bool isSaving = false;
    public NavMeshAgent agent;
    public Vector3 despawnPos;
    public Animator anim;

    private Transform currentInteractObject;

    public AudioClip rescueSfx;

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
        slider.transform.gameObject.SetActive(false);
        oldReleaseTime = releaseTime;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // oldrelease = 6 so 6 == 1 & release time goes down 
        slider.fillAmount = Mathf.InverseLerp(oldReleaseTime, 0, releaseTime);
        if (agent.hasPath)
        {
            Vector3 distanceToEnd = transform.position - despawnPos;
            if (distanceToEnd.magnitude < 1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator StartFreeingHostage()
    {
        isSaving = true;
        SoundManager.Instance.Play(rescueSfx, 0.1f);
        while (currentInteractObject != null)
        {
            slider.transform.gameObject.SetActive(true);
            if (Vector3.Distance(transform.position, currentInteractObject.position) > interactRange)
            {
                SoundManager.Instance.StopSoundEffect(rescueSfx);
                releaseTime = oldReleaseTime;
                OutOfRange?.Invoke();
                slider.transform.gameObject.SetActive(false);
                currentInteractObject = null;
                isSaving = false;
            }
            if(releaseTime <= 0)
            {
                anim.SetBool("isFree", true);
                currentInteractObject = null;
                SavedHostage?.Invoke();
                slider.transform.gameObject.SetActive(false);
                agent.SetDestination(despawnPos);
            }
            releaseTime -= 0.10f;
            yield return new WaitForSeconds(0.10f);
        }
    }
}
