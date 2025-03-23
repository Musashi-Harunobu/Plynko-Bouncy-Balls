using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // -----------------------------
    // ПЕРМАНЕНТНЫЕ (СОХРАНЯЕМЫЕ) ПАРАМЕТРЫ
    // -----------------------------
    
    public int LoadedScene = 1;

    public int Stars;

    public int baseGameBallsOnStart = 2;
    
    private int startSessionStars;
    
    public int sessionStars { get; private set; }
    public int AddBallLevel { get; private set; }
    public int JoinBallLevel { get; private set; }
    public int MultiBallLevel { get; private set; }

    // -----------------------------
    // ТЕКУЩИЕ ПАРАМЕТРЫ СЕССИИ (сбрасываются при GameOver / StartNewGame)
    // -----------------------------

    public int CurrentGameScore;

    public List<BallType> permanentBalls = new List<BallType>();

    public List<BallType> roundBalls = new List<BallType>();
    
    private const int TOP_SIZE = 5;
    public HighScoreEntry[] topResults = new HighScoreEntry[TOP_SIZE];

    private PointManager _pointManager;
    
    private StarSpawner _starSpawner;

    public enum BallType
    {
        Red = 1,
        Purple = 2,
        Yellow = 3,
        Green = 4
    }

    // -----------------------------
    // SINGLETON + ЗАГРУЗКА ДАННЫХ
    // -----------------------------
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Загружаем перманентные данные (Stars, baseGameBallsOnStart, уровни апгрейдов, LoadedScene)
            LoadGameData();
        }
        else
        {
            Destroy(gameObject);
        }
        LoadTopResults();
    }

    private void Start()
    {
        // Если вдруг уровни загрузились как 0
        if (AddBallLevel < 1) AddBallLevel = 1;
        if (JoinBallLevel < 1) JoinBallLevel = 1;
        if (MultiBallLevel < 1) MultiBallLevel = 1;

        // По желанию можно автоматически начать игру
        // StartNewGame();
        
        
    }
    
    public void StartNewGame()
    {
        Debug.Log("StartNewGame(): сбрасываем временные параметры и формируем базовые мячи.");

        CurrentGameScore = 0;
        
        startSessionStars = Stars;

        sessionStars = 0;

        StarSpawner.CollectedStars = 0;
        
        permanentBalls.Clear();
        for (int i = 0; i < baseGameBallsOnStart; i++)
        {
            permanentBalls.Add(BallType.Red);
        }

        // Очищаем roundBalls (на случай, если что-то там было)
        roundBalls.Clear();
        
        if (_starSpawner != null)
        {
            _starSpawner = FindObjectOfType<StarSpawner>();
        }
        
        if (_starSpawner != null)
        {
            _starSpawner.ResetSpawner();
        }

        StartNewRound();
    }


    public void StartNewRound()
    {
        Debug.Log("StartNewRound(): формируем roundBalls из permanentBalls.");

        

        roundBalls.Clear();

        foreach (var ball in permanentBalls)
        {
            roundBalls.Add(ball);
        }

        // Сдвигаем / создаём точки (ваша логика)
        if (_pointManager == null)
            _pointManager = FindObjectOfType<PointManager>();
        if (_pointManager != null)
            _pointManager.MovePointsUp();
        
        _starSpawner = FindObjectOfType<StarSpawner>();
        
        if (_starSpawner != null)
        {
            int randomCount = Random.Range(1, 4);
            _starSpawner.SpawnRandomStars(randomCount);
        }

        Debug.Log($"Начался новый раунд! roundBalls={roundBalls.Count}, permanentBalls={permanentBalls.Count}");
    }

    public void CheckEndRound()
    {
        if (roundBalls.Count == 0)
        {
            Debug.Log("Раунд завершён!");
            StartNewRound();
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER: сбрасываем временные вещи, но сохраняем уровни апгрейдов.");
        
        AddNewResult(CurrentGameScore, sessionStars);

        // Сброс временных составляющих (permanentBalls, Score).
        permanentBalls.Clear();
        roundBalls.Clear();
        CurrentGameScore = 0;
        sessionStars = 0;
        startSessionStars = Stars;
        StarSpawner.CollectedStars = 0;

        for (int i = 0; i < baseGameBallsOnStart; i++)
           permanentBalls.Add(BallType.Red);

        // Сохраняем (уровни апгрейдов, Stars, LoadedScene)
        SaveGameData();
    }
    
    public void AddStars(int value)
    {
        // Увеличиваем общее число звёзд
        Stars += value;

        // Вычисляем, сколько звёзд накоплено за сессию:
        sessionStars = Stars - startSessionStars;

        Debug.Log($"Добавлено {value} звёзд. Всего теперь {Stars}, за сессию {sessionStars}.");
    }
    
    public void RemoveBallFromRoundBalls(BallType type)
    {
        int index = roundBalls.IndexOf(type);
        if (index >= 0)
        {
            roundBalls.RemoveAt(index);
            Debug.Log($"Мяч цвета {type} удалён из roundBalls. Осталось {roundBalls.Count}.");
        }
        else
        {
            Debug.LogWarning($"Попытка удалить мяч цвета {type}, но его нет в roundBalls!");
        }
        

        // Теперь проверяем, не пора ли завершать раунд
        CheckEndRound();
    }

    // -----------------------------
    // ЛОГИКА «ПОКУПОК» И «ОБЪЕДИНЕНИЙ»
    // -----------------------------

    public void BuyAddBall()
    {
        if (sessionStars >= 5)
        {
            int ballsToBuy = AddBallLevel; // например, если уровень=2, покупаем 2 мяча
            for (int i = 0; i < ballsToBuy; i++)
            {
                permanentBalls.Add(BallType.Red);
                roundBalls.Add(BallType.Red); // чтобы сразу использовать в раунде
            }

            sessionStars -= 5;
            // Stars -= ballsToBuy * 10;
            //FindObjectOfType<BallsList2D>().RefreshUI();

            Debug.Log($"Куплено {ballsToBuy} красных мячей. permanentBalls={permanentBalls.Count}, roundBalls={roundBalls.Count}");
        }
        
    }

    /// <summary>
    /// Покупка универсального мяча (Green).
    /// Тоже считаем его «навсегда» (до GameOver).
    /// </summary>
    public void BuyUniversalBall()
    {
        permanentBalls.Add(BallType.Green);
        roundBalls.Add(BallType.Green);
        //FindObjectOfType<BallsList2D>().RefreshUI();
        
        Debug.Log($"Куплен универсальный (зелёный) мяч. permanentBalls={permanentBalls.Count}, roundBalls={roundBalls.Count}");
    }

    /// <summary>
    /// Покупка «MultiBall» — пример. Считаем, что за уровень MultiBallLevel 
    /// добавляем столько же шаров Green.
    /// </summary>
    public void BuyMultiBall()
    {
        if (sessionStars >= 10)
        {
            int multiBallsCount = MultiBallLevel;
            for (int i = 0; i < multiBallsCount; i++)
            {
                permanentBalls.Add(BallType.Green);
                roundBalls.Add(BallType.Green);
            }
            sessionStars -= 10;
            Debug.Log($"Куплено {multiBallsCount} Multi-шаров. permanentBalls={permanentBalls.Count}, roundBalls={roundBalls.Count}");
        }
    }

    public void JoinBalls()
    {
        if (sessionStars >= 7)
        {
            if (JoinBallLevel >= 1)
            {
                // Сколько мячей нужно объединить (2^level)
                int ballsToCombine = (int)Mathf.Pow(2, JoinBallLevel);

                Dictionary<BallType, int> colorCounts = new Dictionary<BallType, int>();
                foreach (var ball in permanentBalls)
                {
                    if (!colorCounts.ContainsKey(ball))
                        colorCounts[ball] = 0;
                    colorCounts[ball]++;
                }

                BallType? foundColor = null;
                foreach (var pair in colorCounts)
                {
                    if (pair.Value >= ballsToCombine)
                    {
                        foundColor = pair.Key; 
                        break;
                    }
                }

                if (foundColor == null)
                {
                    Debug.LogWarning("Нет достаточно одинаковых шаров для объединения!");
                    return;
                }

                BallType colorToCombine = foundColor.Value;

                int removed = 0;
                for (int i = 0; i < permanentBalls.Count && removed < ballsToCombine; i++)
                {
                    if (permanentBalls[i] == colorToCombine)
                    {
                        permanentBalls.RemoveAt(i);
                        i--;
                        removed++;
                    }
                }

                int newBallLevel = (int)colorToCombine + JoinBallLevel;
                if (newBallLevel > (int)BallType.Green)
                {
                    Debug.LogWarning("Превышен максимальный уровень шаров!");
                    return;
                }

                BallType newType = (BallType)newBallLevel;
                permanentBalls.Add(newType);

                roundBalls.Clear();
                roundBalls.AddRange(permanentBalls);

                Debug.Log($"Объединены {ballsToCombine} шаров типа {colorToCombine} => новый {newType}.");

                sessionStars -= 7;
            }
        }
    }


    // -----------------------------
    // ВЫСТРЕЛЫ / ИСПОЛЬЗОВАНИЕ МЯЧЕЙ В РАУНДЕ
    // -----------------------------
    public void SpendBall(int index)
    {
        if (index >= 0 && index < roundBalls.Count)
        {
            roundBalls.RemoveAt(index);
            Debug.Log($"Мяч {index} из roundBalls израсходован. Осталось {roundBalls.Count} мячей в раунде.");
        }
    }

    // -----------------------------
    // ПЕРМАНЕНТНЫЕ АПГРЕЙДЫ
    // -----------------------------

    public void UpgradeAddBall()
    {
        if (Stars >= 20)
        {
            if (AddBallLevel < 3)
            {
                AddBallLevel++;
                SaveGameData();
                Debug.Log($"Улучшен AddBall до уровня {AddBallLevel} (перманентно).");
            }

            Stars -= 20;
        }
        
    }

    public void UpgradeJoinBall()
    {
        if (Stars >= 20)
        {
            if (JoinBallLevel < 3)
            {
                JoinBallLevel++;
                SaveGameData();
                Debug.Log($"Улучшен JoinBall до уровня {JoinBallLevel} (перманентно).");
            }
            Stars -= 20;
        }
        
    }

    public void UpgradeMultiBall()
    {
        if (Stars >= 50)
        {
            if (MultiBallLevel < 3)
            {
                MultiBallLevel++;
                SaveGameData();
                Debug.Log($"Улучшен MultiBall до уровня {MultiBallLevel} (перманентно).");
            }
            Stars -= 50;
        }
        
    }

    // -----------------------------
    // СОХРАНЕНИЕ / ЗАГРУЗКА
    // -----------------------------

    public void SaveGameData()
    {
        PlayerPrefs.SetInt("Stars", Stars);
        PlayerPrefs.SetInt("LoadedScene", LoadedScene);
        PlayerPrefs.SetInt("BaseBallsOnStart", baseGameBallsOnStart);

        PlayerPrefs.SetInt("AddBallLevel", AddBallLevel);
        PlayerPrefs.SetInt("JoinBallLevel", JoinBallLevel);
        PlayerPrefs.SetInt("MultiBallLevel", MultiBallLevel);
        
        PlayerPrefs.SetInt("LoadedScene", LoadedScene);

        PlayerPrefs.Save();
        Debug.Log("Игровые данные сохранены.");
    }

    private void LoadGameData()
    {
        Stars = PlayerPrefs.GetInt("Stars", Stars);
        LoadedScene = PlayerPrefs.GetInt("LoadedScene", LoadedScene);
        baseGameBallsOnStart = PlayerPrefs.GetInt("BaseBallsOnStart", baseGameBallsOnStart);

        AddBallLevel = PlayerPrefs.GetInt("AddBallLevel", 1);
        JoinBallLevel = PlayerPrefs.GetInt("JoinBallLevel", 1);
        MultiBallLevel = PlayerPrefs.GetInt("MultiBallLevel", 1);

        Debug.Log($"Игровые данные загружены: Stars={Stars}, LoadedScene={LoadedScene}, BaseBallsOnStart={baseGameBallsOnStart}, " +
                  $"AddBallLevel={AddBallLevel}, JoinBallLevel={JoinBallLevel}, MultiBallLevel={MultiBallLevel}");
    }
    
    private void LoadTopResults()
    {
        for (int i = 0; i < TOP_SIZE; i++)
        {
            // Считаем Score и Reward из PlayerPrefs
            int score = PlayerPrefs.GetInt($"HS_Score_{i}", 0);
            int reward = PlayerPrefs.GetInt($"HS_Reward_{i}", 0);

            topResults[i] = new HighScoreEntry(score, reward);
        }
    }

    private void SaveTopResults()
    {
        for (int i = 0; i < TOP_SIZE; i++)
        {
            PlayerPrefs.SetInt($"HS_Score_{i}", topResults[i].score);
            PlayerPrefs.SetInt($"HS_Reward_{i}", topResults[i].reward);
        }
        PlayerPrefs.Save();
    }
    
    public void AddNewResult(int newScore, int newReward)
    {
        // 1) Ищем позицию, куда вставить результат
        // (ищем, где score меньше, чем newScore)
        int insertPos = -1;
        for (int i = 0; i < TOP_SIZE; i++)
        {
            if (newScore > topResults[i].score)
            {
                insertPos = i;
                break;
            }
        }

        // Если insertPos так и остался -1, значит newScore не лучше ни одной позиции — не вставляем
        if (insertPos == -1)
        {
            return; 
        }

        // 2) Сдвигаем все результаты «вниз», чтобы освободить место
        for (int i = TOP_SIZE - 1; i > insertPos; i--)
        {
            topResults[i] = topResults[i - 1];
        }

        // 3) Ставим новый результат
        topResults[insertPos] = new HighScoreEntry(newScore, newReward);

        // 4) Сохраняем обновлённую таблицу
        SaveTopResults();
    }
}
