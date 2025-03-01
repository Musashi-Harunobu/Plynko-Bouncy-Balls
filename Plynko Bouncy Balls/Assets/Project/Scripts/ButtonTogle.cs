using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTogle : MonoBehaviour
{
    [SerializeField] private GameObject firstShopPage;
    [SerializeField] private GameObject secondShopPage;

    private bool _isActive = true;

    public void Toggle()
    {
        if (_isActive)
        {
            firstShopPage.SetActive(false);
            secondShopPage.SetActive(true);
            _isActive = false;
        }
        else
        {
            firstShopPage.SetActive(true);
            secondShopPage.SetActive(false);
            _isActive = true;
        }
    }
}
