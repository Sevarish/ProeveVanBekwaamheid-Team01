using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        player.GameOver += ResetScene;
    }

    public void ResetScene() => StartCoroutine(ResetSceneTimer());

    private IEnumerator ResetSceneTimer()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
