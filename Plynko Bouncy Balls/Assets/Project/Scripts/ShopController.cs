using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public enum BallType { Red = 1, Purple = 2, Yellow = 3, Green = 4 }

    private List<BallType> playerBalls = new List<BallType>();

    private int addBallLevel = 0;
    private int joinBallLevel = 0;
    private int multiBallLevel = 0;
    
    public void BuyBall()
    {
        int ballsToBuy = GetBallsToBuy(); 
        for (int i = 0; i < ballsToBuy; i++)
        {
            playerBalls.Add(BallType.Red);
        }
        Debug.Log($"Куплено мячей: {ballsToBuy}");
    }

    public void JoinBalls()
    {
        if (playerBalls.Count >= Mathf.Pow(2, joinBallLevel))
        {
            float ballsToJoin = Mathf.Pow(2, joinBallLevel); 
            List<BallType> ballsToRemove = new List<BallType>();
            BallType resultingBall = BallType.Red;

            foreach (var ball in playerBalls)
            {
                if (ball == resultingBall)
                {
                    ballsToRemove.Add(ball);
                    if (ballsToRemove.Count == ballsToJoin)
                    {
                        break;
                    }
                }
            }

            if (ballsToRemove.Count == ballsToJoin)
            {
                foreach (var ball in ballsToRemove)
                {
                    playerBalls.Remove(ball);
                }

                resultingBall = (BallType)((int)resultingBall + 1);
                playerBalls.Add(resultingBall);

                Debug.Log($"Объединено мячей: {ballsToJoin}, создан новый мяч: {resultingBall}");
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

    public void GetMultiBall()
    {
        int multiBallsToGet = GetMultiBallsToGet(); 
        for (int i = 0; i < multiBallsToGet; i++)
        {
            playerBalls.Add(BallType.Green);
        }
        Debug.Log($"Получено мульти-мячей: {multiBallsToGet}");
    }

    // Прокачка улучшений
    public void UpgradeAddBall()
    {
        if (addBallLevel < 3)
        {
            addBallLevel++;
            Debug.Log($"Улучшение Add Ball прокачано до уровня {addBallLevel}");
        }
        else
        {
            Debug.LogWarning("Максимальный уровень Add Ball已经达到!");
        }
    }

    public void UpgradeJoinBall()
    {
        if (joinBallLevel < 3)
        {
            joinBallLevel++;
            Debug.Log($"Улучшение Join Ball прокачано до уровня {joinBallLevel}");
        }
        else
        {
            Debug.LogWarning("Максимальный уровень Join Ball已经达到!");
        }
    }

    public void UpgradeMultiBall()
    {
        if (multiBallLevel < 3)
        {
            multiBallLevel++;
            Debug.Log($"Улучшение Multi Ball прокачано до уровня {multiBallLevel}");
        }
        else
        {
            Debug.LogWarning("Максимальный уровень Multi Ball已经达到!");
        }
    }
    private int GetBallsToBuy()
    {
        return addBallLevel + 1;
    }

    private int GetMultiBallsToGet()
    {
        return multiBallLevel + 1;
    }
}