using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SceneShop : MonoBehaviour
{
    [SerializeField] private GameObject lessMoneyObject;
    [SerializeField] private GameObject confirmIcon;

    public void IsConfirming(int sceneID)
    {
        if (GameManager.Instance.Stars <= 100)
        {
            lessMoneyObject.SetActive(true);
        }
        else if (GameManager.Instance.Stars >= 100)
        {
            GameManager.Instance.LoadedScene = sceneID;
        }
    }
}
