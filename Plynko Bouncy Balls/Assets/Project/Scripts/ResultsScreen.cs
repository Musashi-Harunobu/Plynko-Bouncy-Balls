using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private GameObject resultsPanel;
    
    [Header("Texts to Show Scores")]
    [SerializeField] private TextMeshProUGUI yourScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    [Header("Buttons")]
    [SerializeField] private GameObject takeButton;

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

    public void OnTakeRewards()
    {
        if (rewardAvailable)
        {
            GameManager.Instance.permanentBalls.Add(GameManager.BallType.Green);
            GameManager.Instance.AddStars(20);

            rewardAvailable = false;
            takeButton.SetActive(false);

            Debug.Log("Награда выдана: +1 шар, +20 звёзд");
        }
    }

    private bool CheckSideQuestComplete()
    {
        return StarSpawner.CollectedStars >= 20;
    }

    private int GetTotalScore()
    {
        return GameManager.Instance.topResults[0].score; 
    }
}
