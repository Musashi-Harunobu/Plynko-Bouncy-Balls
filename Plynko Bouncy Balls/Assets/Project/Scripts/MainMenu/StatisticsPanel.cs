using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] scoreTexts;   // 5 шт.
    [SerializeField] private TextMeshProUGUI[] rewardTexts;  // 5 шт.


    public void ShowStatistics()
    {
        var gm = GameManager.Instance;
        // Предположим, gm.topResults = 5 записей
        for (int i = 0; i < 5; i++)
        {
            scoreTexts[i].text = gm.topResults[i].score.ToString();
            rewardTexts[i].text = $"+{gm.topResults[i].reward}";
        }
    }
    
    public void OnStatisticsButtonClick()
    {
        // Показываем окно (this.gameObject.SetActive(true); например)
        ShowStatistics();
    }
}
