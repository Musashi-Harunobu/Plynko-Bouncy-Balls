using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pointPrefab;
    [SerializeField] private int rows = 1;
    [SerializeField] private int columns = 1;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private float pointSpacing = 0.2f;
    [SerializeField] private GameObject resultCanvas;
    
    private List<GameObject> _points = new List<GameObject>();
    private int _pointCount = 0;
    private bool _colorChange = false;

    private void Awake()
    {
        startPoint = transform.position;
        
        //InvokeRepeating("MovePointsUp", spawnInterval, spawnInterval);
    }

    private void SpawnPoints()
    {
        float startX = startPoint.x;
        float startY = startPoint.y;

        for (int row = 0; row < rows; row++)
        {
            int pointsInRow = columns - row;
            float rowWidth = (pointsInRow - 1) * pointSpacing;
        
            float offsetX = startX - rowWidth / 2;

            for (int col = 0; col < pointsInRow; col++)
            {
                Vector2 position = new Vector2(offsetX + col * pointSpacing, startY - row * pointSpacing);
                GameObject point = Instantiate(pointPrefab[_pointCount], position, Quaternion.identity);
                _points.Add(point);
            }
        }
    }

    public void MovePointsUp()
    {
        foreach (var point in _points)
        {
            if (point != null)
            {
                point.transform.position += Vector3.up * pointSpacing;
            }
        }

        columns++;

        if (columns > 5)
        {
            columns = 1;
        }

        if (_colorChange)
        {
            _pointCount++;

            if (_pointCount >= pointPrefab.Length)
            {
                _pointCount = 0;
            }
        }
        
        _colorChange = !_colorChange;

        SpawnPoints();
        CheckForOverflow();
    }

    private void CheckForOverflow()
    {
        foreach (var point in _points)
        {
            if (point != null && point.transform.position.y >= 3.5f)
            {
                Debug.Log("Игра окончена!");
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        CancelInvoke("MovePointsUp");
        
        ResultsScreen results = FindObjectOfType<ResultsScreen>(true);
        if (results != null)
        {
            results.ShowResults();
        }
        
        GameManager.Instance.GameOver();
        
        
        
        Debug.LogError("Точки достигли верха поля! Игра окончена.");
    }
}