using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentLevel = 0;
    public float goalSize = 1f;
    public PlayerController player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void WinLevel()
    {
        if (player.size < goalSize)
        {
            return;
        }

        if (currentLevel < 5)
        {
            ++currentLevel;
            SceneManager.LoadScene("Level " + currentLevel);
        }
        else
        {
            SceneManager.LoadScene("End Screen");
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
