using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Player player;
    private ObjectiveManager gameState;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        gameState = GameObject.FindObjectOfType<ObjectiveManager>();
        player.GameOver += ResetScene;
        gameState.GameWon += FinishScene;
    }

    public void ResetScene() => StartCoroutine(ResetSceneTimer());
    public void FinishScene() => StartCoroutine(CompleteSceneTimer());

    private IEnumerator ResetSceneTimer()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator CompleteSceneTimer()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("WinScreen");
    }
}
