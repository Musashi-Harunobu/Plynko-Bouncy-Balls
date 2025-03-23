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
                var ballType = ballComponent.ballType;
                Destroy(other.gameObject);

                GameManager.Instance.roundBalls.Add(ballType);

                // При желании здесь тоже можно давать очки/звёзды, если нужно
                // GameManager.Instance.CurrentGameScore += holeValue;
                // GameManager.Instance.Stars += holeValue;
                
                //FindObjectOfType<BallsList2D>().RefreshUI();

                _scaleManager.ChangeScale();
            }
            else
            {
                // Обычная лунка: шар уничтожается
                Destroy(other.gameObject);

                // Ищем первый мяч такого цвета в roundBalls и удаляем
                var ballType = ballComponent.ballType;
                GameManager.Instance.RemoveBallFromRoundBalls(ballType);

                // Начисляем очки/звёзды
                GameManager.Instance.CurrentGameScore += holeValue;
                GameManager.Instance.AddStars(holeValue);

                Debug.Log($"Score={GameManager.Instance.CurrentGameScore} Stars={GameManager.Instance.Stars}");

                _scaleManager.ChangeScale();
            }
        }
        //FindObjectOfType<BallsList2D>().RefreshUI();
    }
}