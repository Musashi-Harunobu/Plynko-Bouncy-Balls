using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stars;
    
    [SerializeField] private TextMeshProUGUI currentGameScoreText;
    [SerializeField] private TextMeshProUGUI pausedGameScoreText;
    [SerializeField] private TextMeshProUGUI roundStarCountText;
    
    [SerializeField] private TextMeshProUGUI resultGameScoreText;
    [SerializeField] private TextMeshProUGUI collectedStarsText;

    private void FixedUpdate()
    {
        if (stars != null)
        {
            stars.text = $"{GameManager.Instance.Stars}";
        }
        

        if (currentGameScoreText != null)
        {
            currentGameScoreText.text = $"{GameManager.Instance.CurrentGameScore}";
        }
        if (roundStarCountText != null)
        {
            roundStarCountText.text = $"{GameManager.Instance.sessionStars}";
        }
        if (pausedGameScoreText != null)
        {
            pausedGameScoreText.text = $"{GameManager.Instance.CurrentGameScore}";
        }
        if (collectedStarsText != null)
        {
            collectedStarsText.text = $"{StarSpawner.CollectedStars}/20";
        }
    }
}
