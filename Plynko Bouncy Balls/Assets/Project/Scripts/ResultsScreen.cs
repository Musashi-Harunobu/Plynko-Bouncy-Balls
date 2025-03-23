using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private GameObject resultsPanel;
    
    [Header("Texts to Show Scores")]
    [SerializeField] private TextMeshProUGUI yourScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Buttons")]
    [SerializeField] private GameObject takeButton;     // Кнопка TAKE!

    private bool rewardAvailable = false;

    /// <summary>
    /// Вызывается, когда игра заканчивается (игрок проиграл).
    /// Отображает окно и заполняет поля UI из GameManager.
    /// </summary>
    public void ShowResults()
    {
        resultsPanel.SetActive(true);

        yourScoreText.text = GameManager.Instance.CurrentGameScore.ToString();

        totalScoreText.text = GetTotalScore().ToString();
        
        if (CheckSideQuestComplete())
        {
            rewardAvailable = true;
        }
        else
        {
            rewardAvailable = false;
        }
        
    }

    // Этот метод можно вызвать из кнопки "TAKE!"
    public void OnTakeRewards()
    {
        if (rewardAvailable)
        {
            // Начисляем награду 
            // (напр., +1 в permanentBalls, +20 в Stars)
            GameManager.Instance.permanentBalls.Add(GameManager.BallType.Green);
            GameManager.Instance.AddStars(20);

            // Предотвращаем повторное «взятие»
            rewardAvailable = false;
            takeButton.SetActive(false);

            Debug.Log("Награда выдана: +1 шар, +20 звёзд");
        }
    }

    private bool CheckSideQuestComplete()
    {
        // Здесь своя логика проверки: собрал ли игрок все 20 звёзд?
        // К примеру:
        // return (StarSpawner.CollectedStars >= 20);
        return StarSpawner.CollectedStars >= 20;
    }

    private int GetTotalScore()
    {
        // Если под "TOTAL" подразумевается общее кол-во очков 
        // за все игры? Или текущие Stars?
        // Предположим, это просто Stars:
        return GameManager.Instance.topResults[0].score; 
    }
}
