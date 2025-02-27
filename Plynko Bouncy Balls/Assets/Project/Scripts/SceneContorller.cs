using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContorller : MonoBehaviour
{
    public static int CurrentSceneID()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void Restart()
    {
        SceneManager.LoadScene(CurrentSceneID());
        Time.timeScale = 1f;
    }

    public static void LoadNextScene()
    {
        int sceneID = CurrentSceneID();

        SceneManager.LoadScene(++sceneID);
    }

    public static void LoadSceneByIndex(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public static void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void CloseGame()
    {
        Application.Quit();
    }
}
