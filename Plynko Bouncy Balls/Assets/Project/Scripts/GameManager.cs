using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Stars;
    public int GameBallsOnStart = 2;
    public int CurrentGameScore;

    // Перманентные улучшения (из магазина)
    public int AddBallLevel { get; private set; }
    public int JoinBallLevel { get; private set; }
    public int MultiBallLevel { get; private set; }

    public enum BallType { Red = 1, Purple = 2, Yellow = 3, Green = 4 }

    public List<BallType> playerBalls = new List<BallType>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PlayerPrefs.DeleteAll();
        LoadGameData();
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetInt("Stars", Stars);
        PlayerPrefs.SetInt("GameBallsOnStart", GameBallsOnStart);
        PlayerPrefs.SetInt("AddBallLevel", AddBallLevel);
        PlayerPrefs.SetInt("JoinBallLevel", JoinBallLevel);
        PlayerPrefs.SetInt("MultiBallLevel", MultiBallLevel);
        PlayerPrefs.Save();
        Debug.Log("Игровые данные сохранены.");
    }

    private void LoadGameData()
    {
        Stars = PlayerPrefs.GetInt("Stars", 0);
        GameBallsOnStart = PlayerPrefs.GetInt("GameBallsOnStart", 2);
        AddBallLevel = PlayerPrefs.GetInt("AddBallLevel", 0);
        JoinBallLevel = PlayerPrefs.GetInt("JoinBallLevel", 0);
        MultiBallLevel = PlayerPrefs.GetInt("MultiBallLevel", 0);
        Debug.Log($"Игровые данные загружены. Звезды = {Stars}, Стартовые мячи = {GameBallsOnStart}");
    }
    
    public void StartNewRound()
    {
        playerBalls.Clear();
        for (int i = 0; i < GameBallsOnStart; i++)
        {
            playerBalls.Add(BallType.Red); // Добавляем 2 красных шара
        }
        Debug.Log($"Новый раунд! Выдано {GameBallsOnStart} красных шара.");
    }
    
    public void CheckEndRound()
    {
        if (playerBalls.Count == 0)
        {
            Debug.Log("Раунд завершен!");
            StartNewRound();
        }
    }

    public void AddBall(BallType type, int count)
    {
        for (int i = 0; i < count; i++)
        {
            playerBalls.Add(type);
        }
        Debug.Log($"Добавлено {count} мячей типа {type}");
    }

    public void UpgradeAddBall()
    {
        if (AddBallLevel < 3)
        {
            AddBallLevel++;
            SaveGameData();
            Debug.Log($"Улучшение Add Ball: Уровень {AddBallLevel}");
        }
    }

    public void UpgradeJoinBall()
    {
        if (JoinBallLevel < 3)
        {
            JoinBallLevel++;
            SaveGameData();
            Debug.Log($"Улучшение Join Ball: Уровень {JoinBallLevel}");
        }
    }

    public void UpgradeMultiBall()
    {
        if (MultiBallLevel < 3)
        {
            MultiBallLevel++;
            SaveGameData();
            Debug.Log($"Улучшение Multi Ball: Уровень {MultiBallLevel}");
        }
    }
}
