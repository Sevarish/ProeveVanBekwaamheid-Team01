using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public TMP_Text hostageCounterText;
    public TMP_Text gameTimerText;
    private Hostage[] hostages;
    public string textBeforeHostageCounter;
    public string textBeforeGameTimer;
    public float gameTimer = 90;
    private int amountOfHostages;
    private int savedHostages;
    public Action GameOver;
    public Action GameWon;

    private void Start()
    {
        hostages = GameObject.FindObjectsOfType<Hostage>();
        foreach(Hostage hostage in hostages)
        {
            hostage.SavedHostage += OnSaved;
        }
        amountOfHostages = hostages.Length;
        StartCoroutine(ReduceGameTime());
        GameOver += RestartLevel;
    }

    private void OnSaved()
    {
        savedHostages++;
    }

    private void Update()
    {
        hostageCounterText.text = textBeforeHostageCounter + savedHostages + "/" + amountOfHostages;
        if (savedHostages == amountOfHostages)
        {
            GameWon?.Invoke();
        }
    }

    private IEnumerator ReduceGameTime()
    {
        while(gameTimer >= 0)
        {
            gameTimer -= 0.1f;
            gameTimerText.text = textBeforeGameTimer + Mathf.Round(gameTimer);
            yield return new WaitForSeconds(0.1f);
        }
        GameOver?.Invoke();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }
}
