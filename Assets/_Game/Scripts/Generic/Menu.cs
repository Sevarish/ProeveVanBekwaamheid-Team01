using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject target;
    public float rotationsPerSecond;

    private AsyncOperation loadData;
    public Slider progressSlider;
    public TMP_Text text;

    public string controlsSceneName = "Controls", gameName = "Game";

    private void Start()
    {
        LoadGame();
    }

    public void GoToControlScheme()
    {
        SceneManager.LoadScene(controlsSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        loadData = SceneManager.LoadSceneAsync(gameName, LoadSceneMode.Single);
        loadData.allowSceneActivation = false;
    }

    private void Update()
    {
        RotateCam();

        if (SceneManager.GetActiveScene().name == controlsSceneName)
        { 
        progressSlider.value = loadData.progress;
        if (loadData.progress >= 0.9f)
        {
            progressSlider.gameObject.SetActive(false);
            text.gameObject.SetActive(true);
        }

            if (Input.GetKeyUp(KeyCode.Space) && loadData.progress >= 0.9f)
            {
                loadData.allowSceneActivation = true;
            }
        }
    }

    private void RotateCam()
    {
        if (target != null)
        {
            Camera.main.transform.RotateAround(target.transform.position, Vector3.up, (360 * rotationsPerSecond) * Time.deltaTime);
        }
    }
}
