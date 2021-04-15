using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ObjectiveManager : MonoBehaviour
{
    public TMP_Text hostageCounter;
    private Hostage[] hostages;
    public string textBeforeCounter;
    private int amountOfHostages;
    private int savedHostages;

    private void Start()
    {
        hostages = GameObject.FindObjectsOfType<Hostage>();
        foreach(Hostage hostage in hostages)
        {
            hostage.SavedHostage += OnSaved;
        }
        amountOfHostages = hostages.Length;
    }

    private void OnSaved()
    {
        savedHostages++;
    }

    private void Update()
    {
        hostageCounter.text = textBeforeCounter + savedHostages + "/" + amountOfHostages;
    }
}
