using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // Скорость поворота пушки

    private float _minAngle = -90f;
    private float _maxAngle = 90f;
    
    private void Update()
    {
        RotateCannon();
    }

    void RotateCannon()
    {
        // Получаем свайп на экране
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            // Поворачиваем пушку в зависимости от движения пальца
            float rotationAmount = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, -rotationAmount);
        }
    }
}