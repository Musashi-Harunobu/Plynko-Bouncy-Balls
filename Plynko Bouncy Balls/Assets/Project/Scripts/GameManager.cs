using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Stars;
    public int GameBallsOnStart = 2;
    public int CurrentGameScore;

    public int LoadedScene = 1;

    // Перманентные улучшения (из магазина)
    public int AddBallLevel { get; private set; }
    public int JoinBallLevel { get; private set; }
    public int MultiBallLevel { get; private set; }

    public enum BallType { Red = 1, Purple = 2, Yellow = 3, Green = 4 }

    public List<BallType> playerBalls = new List<BallType>();
    
    private PointManager _pointManager;

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
        
        

        //PlayerPrefs.DeleteAll();
        //LoadGameData();
    }

    private void Start()
    {
        AddBallLevel = 1;
        JoinBallLevel = 1;
        MultiBallLevel = 1;
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
        Stars = PlayerPrefs.GetInt("Stars", Stars);
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

        if (_pointManager == null)
        {
            _pointManager = FindObjectOfType<PointManager>();
        }
        
        _pointManager.MovePointsUp();
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
    // Механизм покупки дополнительного мяча
    public void BuyAddBall()
    {
        int ballsToBuy = AddBallLevel;  // Количество мячей увеличивается с каждым уровнем
        for (int i = 0; i < ballsToBuy; i++)
        {
            playerBalls.Add(BallType.Red);  // Все мячи красные
        }
        Debug.Log($"Куплено {ballsToBuy} дополнительных мячей.");
    }

    // Объединение мячей
    public void JoinBalls()
    {
        if (JoinBallLevel >= 1)
        {
            int ballsToCombine = (int)Mathf.Pow(2, JoinBallLevel); // Количество мячей для объединения, зависит от уровня улучшения
            if (playerBalls.Count >= ballsToCombine)
            {
                // Подсчитываем, сколько мячей одного уровня нужно для объединения
                int count = 1;
                GameManager.BallType resultingBall = GameManager.BallType.Red;  // Начальный уровень

                // Ищем мячи нужного уровня
                List<GameManager.BallType> ballsToRemove = new List<GameManager.BallType>();

                for (int i = playerBalls.Count - 1; i >= 0; i--)
                {
                    if (playerBalls[i] == resultingBall)
                    {
                        ballsToRemove.Add(playerBalls[i]);
                        count++;
                        if (count == ballsToCombine)
                        {
                            break;
                        }
                    }
                }

                // Если нашли нужное количество мячей, объединяем
                if (count == ballsToCombine)
                {
                    foreach (var ball in ballsToRemove)
                    {
                        playerBalls.Remove(ball);
                    }
                
                    resultingBall = (GameManager.BallType)((int)resultingBall + 1);  // Повышаем уровень мяча
                    playerBalls.Add(resultingBall);
                
                    Debug.Log($"Объединены {ballsToCombine} мяча(ей) в новый мяч уровня {resultingBall}");
                }
                else
                {
                    Debug.LogWarning("Недостаточно мячей для объединения.");
                }
            }
            else
            {
                Debug.LogWarning("Недостаточно мячей для объединения.");
            }
        }
    }


    // Механизм покупки универсального мяча
    public void BuyUniversalBall()
    {
        playerBalls.Add(GameManager.BallType.Green); // Универсальный мяч добавляется в список
        Debug.Log("Куплен универсальный мяч, который наносит максимальный урон.");
    }
    
    // Обновление уровня улучшения
    public void UpgradeAddBall()
    {
        if (AddBallLevel <= 3)
        {
            AddBallLevel++;
            SaveGameData();
            Debug.Log($"Улучшение Add Ball: Уровень {AddBallLevel}");
        }
    }

    public void UpgradeJoinBall()
    {
        if (JoinBallLevel <= 3)
        {
            JoinBallLevel++;
            SaveGameData();
            Debug.Log($"Улучшение Join Ball: Уровень {JoinBallLevel}");
        }
    }

    public void UpgradeMultiBall()
    {
        if (MultiBallLevel <= 3)
        {
            MultiBallLevel++;
            SaveGameData();
            Debug.Log($"Улучшение Multi Ball: Уровень {MultiBallLevel}");
        }
    }
}
