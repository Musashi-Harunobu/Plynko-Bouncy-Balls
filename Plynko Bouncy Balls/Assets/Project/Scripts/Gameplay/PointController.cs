using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class PointController : MonoBehaviour
{ 
    [SerializeField] private TextMeshPro strengthText;
    [SerializeField] private string tagName;

    private int _strength;

    private void Awake()
    {
        _strength = Random.Range(1, 8);
    }

    private void Update()
    {
        strengthText.text = _strength.ToString();
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagName))
        {
            _strength--;
    
            if (_strength <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}