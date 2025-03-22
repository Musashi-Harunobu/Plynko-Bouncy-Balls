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

        // Сброс временных составляющих (permanentBalls, Score).
        permanentBalls.Clear();
        roundBalls.Clear();
        CurrentGameScore = 0;

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

    /// <summary>
    /// Объединение мячей. Важно делать это в permanentBalls, чтобы результат 
    /// сохранился на будущие раунды. При желании — синхронизировать roundBalls.
    /// </summary>
    public void JoinBalls()
    {
        if (sessionStars >= 7)
        {
            if (JoinBallLevel >= 1)
            {
                int ballsToCombine = (int)Mathf.Pow(2, JoinBallLevel); 
                // допустим, если JoinBallLevel=2 => нужно 4 одинаковых шара

                if (permanentBalls.Count >= ballsToCombine)
                {
                    // Для упрощения возьмём первые 'ballsToCombine' шара
                    BallType firstBallType = permanentBalls[0];
                    bool allSame = true;
                    for (int i = 1; i < ballsToCombine; i++)
                    {
                        if (permanentBalls[i] != firstBallType)
                        {
                            allSame = false;
                            break;
                        }
                    }
                    if (!allSame)
                    {
                        Debug.LogWarning("Мячи разных типов, нельзя объединить.");
                        return;
                    }

                    // Удаляем их
                    permanentBalls.RemoveRange(0, ballsToCombine);

                    // Создаём новый шар
                    int newLevel = (int)firstBallType + JoinBallLevel;
                    if (newLevel > (int)BallType.Green)
                    {
                        Debug.LogWarning("Превышен максимальный уровень шаров!");
                        return;
                    }
                    BallType newType = (BallType)newLevel;
                    permanentBalls.Add(newType);

                    // Обновляем roundBalls, чтобы изменения отразились прямо сейчас
                    roundBalls.Clear();
                    roundBalls.AddRange(permanentBalls);

                    Debug.Log($"Объединены {ballsToCombine} шаров типа {firstBallType} => новый {newType}.");
                }
                else
                {
                    Debug.LogWarning("Недостаточно мячей для объединения!");
                }

                sessionStars -= 7;
            }
        }
        
    }

    // -----------------------------
    // ВЫСТРЕЛЫ / ИСПОЛЬЗОВАНИЕ МЯЧЕЙ В РАУНДЕ
    // -----------------------------

    /// <summary>
    /// Когда игрок делает выстрел и «тратит» мяч, удаляем его ИЗ roundBalls, 
    /// но не из permanentBalls (если логика «мяч потрачен окончательно»).
    /// Если по механике мяч вернулся через красную лунку, можно заново добавить его 
    /// в roundBalls. 
    /// </summary>
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
}
