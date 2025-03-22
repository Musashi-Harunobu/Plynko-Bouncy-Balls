using UnityEngine;

public class HoleManager : MonoBehaviour
{
    [SerializeField] private bool isBigHole; // Укажите в инспекторе, большая (true) или обычная (false).
    [SerializeField] private int holeValue = 10; // Сколько очков/звёзд даёт лунка

    private ScaleManager _scaleManager;

    private void Awake()
    {
        _scaleManager = GetComponent<ScaleManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Проверяем, что в лунку попал шар (Ball)
        Ball ballComponent = other.gameObject.GetComponent<Ball>();
        if (ballComponent != null)
        {
            // Если лунка большая (красная)
            if (isBigHole)
            {
                // Возвращаем шар в список playerBalls
                var ballType = ballComponent.ballType;
                Destroy(other.gameObject); // удаляем физический объект

                // Но добавляем тот же самый тип шара обратно в очередь GameManager
                GameManager.Instance.roundBalls.Add(ballType);

                // При желании здесь тоже можно давать очки/звёзды, если нужно
                // GameManager.Instance.CurrentGameScore += holeValue;
                // GameManager.Instance.Stars += holeValue;

                _scaleManager.ChangeScale();
            }
            else
            {
                // Обычная лунка: шар уничтожается, зарабатываем очки и звёзды
                Destroy(other.gameObject);

                
                GameManager.Instance.CurrentGameScore += holeValue;
                GameManager.Instance.AddStars(holeValue);

                Debug.Log($"Score={GameManager.Instance.CurrentGameScore} Stars={GameManager.Instance.Stars}");

                _scaleManager.ChangeScale();
            }
        }
    }
}