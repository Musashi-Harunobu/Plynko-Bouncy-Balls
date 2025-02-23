using System;
using UnityEngine;
using TMPro;

public class PointController : MonoBehaviour
{
    [SerializeField] private int strength = 3; // Прочность точки
    [SerializeField] private TextMeshPro strengthText;
    [SerializeField] private string tagName;

    private void Update()
    {
        strengthText.text = strength.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagName))
        {
            // Уменьшаем прочность точки при попадании мяча
            strength--;

            if (strength <= 0)
            {
                // Удаляем точку, когда ее прочность достигает нуля
                Destroy(gameObject);
            }
        }
    }
}