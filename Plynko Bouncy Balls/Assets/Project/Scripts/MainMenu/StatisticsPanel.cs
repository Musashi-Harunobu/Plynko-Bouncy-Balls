using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private TextMeshProUGUI[] rewardTexts;


    public void ShowStatistics()
    {
        var gm = GameManager.Instance;
        for (int i = 0; i < 5; i++)
        {
            scoreTexts[i].text = gm.topResults[i].score.ToString();
            rewardTexts[i].text = $"+{gm.topResults[i].reward}";
        }
    }
    
    public void OnStatisticsButtonClick()
    {
        ShowStatistics();
    }
}
