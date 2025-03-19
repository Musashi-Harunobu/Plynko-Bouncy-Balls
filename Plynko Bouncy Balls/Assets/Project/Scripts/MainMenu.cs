using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void FixedUpdate()
    {
        playButton.onClick.AddListener(LoadedSceneController);
    }

    private void LoadedSceneController()
    {
        SceneContorller.LoadSceneByIndex(GameManager.Instance.LoadedScene);
    }
}
