using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void LoadedSceneController()
    {
        SceneContorller.LoadSceneByIndex(GameManager.Instance.LoadedScene);
    }
}
