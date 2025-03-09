using UnityEngine;

public class ShopController : MonoBehaviour
{
    public void BuyBall()
    {
        int ballsToBuy = GameManager.Instance.AddBallLevel + 1;
        GameManager.Instance.AddBall(GameManager.BallType.Red, ballsToBuy);
        Debug.Log($"Куплено мячей: {ballsToBuy}");
    }

    public void JoinBalls()
    {
        int requiredBalls = (int)Mathf.Pow(2, GameManager.Instance.JoinBallLevel);
        if (GameManager.Instance.playerBalls.Count >= requiredBalls)
        {
            GameManager.BallType resultingBall = GameManager.BallType.Red;
            int count = 0;

            for (int i = GameManager.Instance.playerBalls.Count - 1; i >= 0; i--)
            {
                if (GameManager.Instance.playerBalls[i] == resultingBall)
                {
                    GameManager.Instance.playerBalls.RemoveAt(i);
                    count++;
                    if (count == requiredBalls) break;
                }
            }

            resultingBall = (GameManager.BallType)((int)resultingBall + 1);
            GameManager.Instance.AddBall(resultingBall, 1);
            Debug.Log($"Объединено {requiredBalls} мячей -> Новый мяч: {resultingBall}");
        }
        else
        {
            Debug.LogWarning("Недостаточно мячей для объединения.");
        }
    }

    public void GetMultiBall()
    {
        int multiBallsToGet = GameManager.Instance.MultiBallLevel + 1;
        GameManager.Instance.AddBall(GameManager.BallType.Green, multiBallsToGet);
        Debug.Log($"Получено мульти-мячей: {multiBallsToGet}");
    }
    
    public void UpgradeAddBall()
    {
        GameManager.Instance.UpgradeAddBall();
    }

    public void UpgradeJoinBall()
    {
        GameManager.Instance.UpgradeJoinBall();
    }

    public void UpgradeMultiBall()
    {
        GameManager.Instance.UpgradeMultiBall();
    }
}