using System;
using System.Collections;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    [SerializeField] private int holeValue;
    
    private ScaleManager _scaleManager;

    private void Awake()
    {
        _scaleManager = GetComponent<ScaleManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Ball>())
        {
            Destroy(other.gameObject);
            GameManager.Instance.CurrentGameScore += holeValue;
            Debug.Log(GameManager.Instance.CurrentGameScore);
            _scaleManager.ChangeScale();
        }
    }
}