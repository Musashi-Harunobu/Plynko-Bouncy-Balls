using System;
using UnityEngine;
using UnityEngine.UI;

public class CannonPowerController : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private GameObject cannonBall;

    private void Start()
    {
        // Инициализация слайдера
        powerSlider.minValue = 0f;
        powerSlider.maxValue = 1f;
    }

    public void FireCannon()
    {
        // Получаем силу выстрела
        float power = Input.GetTouch(0).deltaTime;
        powerSlider.value = power;
        
        // Создаем мяч и добавляем силу
        GameObject ball = Instantiate(cannonBall, transform.position, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(-transform.up * power * 500f); // Добавляем силу в сторону пушки
    }
}