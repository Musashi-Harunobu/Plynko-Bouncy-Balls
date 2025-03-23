using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Button buyAddBallButton;
    [SerializeField] private Button joinBallsButton;
    [SerializeField] private Button buyUniversalBallButton;

    private void Start()
    {
        // Привязываем кнопки к методам
        buyAddBallButton.onClick.AddListener(BuyAddBall);
        joinBallsButton.onClick.AddListener(JoinBalls);
        buyUniversalBallButton.onClick.AddListener(BuyUniversalBall);
    }

    // Метод для покупки дополнительного мяча
    public void BuyAddBall()
    {
        GameManager.Instance.BuyAddBall();
    }

    // Метод для объединения мячей
    public void JoinBalls()
    {
        GameManager.Instance.JoinBalls();
    }

    // Метод для покупки универсального мяча
    public void BuyUniversalBall()
    {
        GameManager.Instance.BuyUniversalBall();
    }
}