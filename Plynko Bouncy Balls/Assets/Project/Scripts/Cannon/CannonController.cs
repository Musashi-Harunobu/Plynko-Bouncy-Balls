using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // Скорость поворота пушки
    
    private void Update()
    {
        RotateCannon();
    }

    private void RotateCannon()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            float rotationAmount = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, -rotationAmount);
        }
    }

    private void RotateCannonWithMouse()
    {
        if (Input.GetMouseButton(0))
        {
            float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, -rotationAmount);
        }
    }
}