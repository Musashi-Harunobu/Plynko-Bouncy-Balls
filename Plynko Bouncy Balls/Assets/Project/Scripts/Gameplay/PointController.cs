using TMPro;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] private GameManager.BallType pointColor; 
    [SerializeField] private TextMeshPro strengthText;

    private int _strength;

    private void Awake()
    {
        // Случайная прочность
        _strength = Random.Range(3, 8);
    }

    private void Update()
    {
        strengthText.text = _strength.ToString();
    }

    // При столкновении с шаром
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            // Универсальный зелёный мяч (уровень 4) — убиваем точку сразу
            if (ball.ballType == GameManager.BallType.Green)
            {
                _strength -= 2;
            }
            // Совпадение цвета — повышенный урон (например, -2)
            else if (ball.ballType == pointColor)
            {
                _strength -= 2;
            }
            // Иначе обычный урон (-1)
            else
            {
                _strength -= 1;
            }

            if (_strength <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}