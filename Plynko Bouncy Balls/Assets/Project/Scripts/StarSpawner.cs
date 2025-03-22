using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    private static int spawnedStarsCount;
    
    [Header("Параметры спавна звёзд")]
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private int maxStarsPerSession = 20;
    
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-5f, -3f);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(5f, 3f);
    
    public void ResetSpawner()
    {
        spawnedStarsCount = 0;
    }

    public void SpawnRandomStars(int starsToSpawn)
    {
        // Сколько ещё можем заспавнить, не превышая лимит
        int available = maxStarsPerSession - spawnedStarsCount;
        if (available <= 0)
        {
            Debug.Log("StarSpawner: достигнут лимит спавна звёзд за сессию.");
            return;
        }

        // Если запросили больше, чем осталось "доступных", уменьшим
        if (starsToSpawn > available)
        {
            starsToSpawn = available;
        }

        for (int i = 0; i < starsToSpawn; i++)
        {
            float randX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPos = new Vector2(randX, randY);

            Instantiate(starPrefab, spawnPos, Quaternion.identity);

            spawnedStarsCount++;
        }
        
        Debug.Log($"StarSpawner: заспавнили {starsToSpawn} звёзд, всего {spawnedStarsCount}/{maxStarsPerSession}.");
    }
}
