using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Stars;
    public int GameBallsOnStart;
    public int CurrentGameScore;

    public enum GameMap
    {
        Argon,
        Meridian,
        Sunset,
        ForestTrail,
        Grassland,
        Midnight
    }

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

        LoadGameData();
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetInt("Stars", Stars);
        PlayerPrefs.SetInt("GameBallsOnStart", GameBallsOnStart);
        PlayerPrefs.Save();
        Debug.Log("Игровые данные сохранены.");
    }

    private void LoadGameData()
    {
        Stars = PlayerPrefs.GetInt("Stars", 0);
        GameBallsOnStart = PlayerPrefs.GetInt("GameBallsOnStart", 1); 
        Debug.Log("Игровые данные загружены. Звезды: " + Stars + ", Мячи на старте: " + GameBallsOnStart);
    }

    public void AddStars(int amount)
    {
        Stars += amount;
        SaveGameData();
        Debug.Log("Добавлено звезд: " + amount + ". Общее количество звезд: " + Stars);
    }

    public void SetGameBallsOnStart(int balls)
    {
        GameBallsOnStart = balls;
        SaveGameData();
        Debug.Log("Установлено мячей на старте: " + GameBallsOnStart);
    }
}