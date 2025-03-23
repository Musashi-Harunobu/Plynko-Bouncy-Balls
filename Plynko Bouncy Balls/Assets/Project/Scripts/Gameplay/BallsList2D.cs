using System.Collections.Generic;
using UnityEngine;

public class BallsList2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform container;
    [SerializeField] private GameObject ballIconPrefab;

    [Header("Sprites for each type")]
    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite purpleSprite;
    [SerializeField] private Sprite yellowSprite;
    [SerializeField] private Sprite greenSprite;

    [Header("Positioning")]
    [SerializeField] private Vector2 startOffset = new Vector2(-7f, 3f);
    [SerializeField] private float xSpacing = 1.0f;
    [SerializeField] private float ySpacing = 0.0f;

    private List<GameObject> spawnedIcons = new List<GameObject>();

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (var icon in spawnedIcons)
        {
            Destroy(icon);
        }
        spawnedIcons.Clear();

        var gm = GameManager.Instance;
        if (gm == null) return;

        var ballList = gm.roundBalls;

        for (int i = 0; i < ballList.Count; i++)
        {
            GameManager.BallType ballType = ballList[i];

            Vector2 offset = new Vector2(xSpacing * i, ySpacing * i);
            Vector3 spawnPos = (Vector3)(startOffset - offset);

            var iconGO = Instantiate(ballIconPrefab, container);

            iconGO.transform.localPosition = spawnPos;

            var sr = iconGO.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = GetSpriteByType(ballType);
            }

            spawnedIcons.Add(iconGO);
        }
    }

    private Sprite GetSpriteByType(GameManager.BallType ballType)
    {
        switch (ballType)
        {
            case GameManager.BallType.Red:    return redSprite;
            case GameManager.BallType.Purple: return purpleSprite;
            case GameManager.BallType.Yellow: return yellowSprite;
            case GameManager.BallType.Green:  return greenSprite;
        }
        return null;
    }
}
