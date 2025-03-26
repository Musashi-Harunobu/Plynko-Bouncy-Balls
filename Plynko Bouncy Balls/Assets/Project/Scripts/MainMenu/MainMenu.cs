using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    
    
    public void LoadedSceneController()
    {
        sceneLoader.LoadScene(GameManager.Instance.LoadedScene);
    }
}
