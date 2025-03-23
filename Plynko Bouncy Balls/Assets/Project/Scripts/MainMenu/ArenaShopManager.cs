using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArenaShopManager : MonoBehaviour
{
    [SerializeField] private GameObject doNotHaveStarsMenu;
    [SerializeField] private GameObject confirmMenu;

    private int _choosenScene;

    public void IsConfirming(int scene)
    {
        if (GameManager.Instance.Stars <= 100)
        {
            doNotHaveStarsMenu.SetActive(true);
        }
        else if (GameManager.Instance.Stars >= 100)
        {
            confirmMenu.SetActive(true);
            _choosenScene = scene;
        }
    }

    public void SetNewLocation()
    {
        GameManager.Instance.LoadedScene = _choosenScene;
    }
}
