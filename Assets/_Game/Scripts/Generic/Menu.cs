using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject target;
    public float rotationsPerSecond;

    public string controlsSceneName = "Controls", gameName = "Game";

    public void GoToControlScheme()
    {
        SceneManager.LoadScene(controlsSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameName);
    }

    private void Update()
    {
        RotateCam();
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == controlsSceneName)
            {
                StartGame();
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
