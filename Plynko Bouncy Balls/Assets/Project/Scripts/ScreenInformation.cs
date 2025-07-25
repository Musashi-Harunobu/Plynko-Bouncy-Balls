using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stars;
    [SerializeField] private TextMeshProUGUI  baseGameStartBalls;
    
    [SerializeField] private TextMeshProUGUI currentGameScoreText;
    [SerializeField] private TextMeshProUGUI pausedGameScoreText;
    [SerializeField] private TextMeshProUGUI roundStarCountText;
    
    [SerializeField] private TextMeshProUGUI resultGameScoreText;
    [SerializeField] private TextMeshProUGUI collectedStarsText;
    
    [SerializeField] private TextMeshProUGUI newScoreText; 
    [SerializeField] private TextMeshProUGUI totalScoreText;

    private void FixedUpdate()
    {
        if (stars != null)
        {
            stars.text = $"{GameManager.Instance.Stars}";
        }

        if (baseGameStartBalls != null)
        {
            baseGameStartBalls.text = $"{GameManager.Instance.baseGameBallsOnStart}";
        }
        

        if (currentGameScoreText != null)
        {
            currentGameScoreText.text = $"{GameManager.Instance.CurrentGameScore}";
        }
        if (roundStarCountText != null)
        {
            roundStarCountText.text = $"{GameManager.Instance.SessionStars}";
        }
        if (pausedGameScoreText != null)
        {
            pausedGameScoreText.text = $"{GameManager.Instance.CurrentGameScore}";
        }
        if (collectedStarsText != null)
        {
            collectedStarsText.text = $"{StarSpawner.CollectedStars}/20";
        }

        if (newScoreText != null)
        {
            newScoreText.text = GameManager.Instance.CurrentGameScore.ToString();
        }

        if (totalScoreText != null)
        {
            totalScoreText.text = GameManager.Instance.Stars.ToString();
        }
    }
}
