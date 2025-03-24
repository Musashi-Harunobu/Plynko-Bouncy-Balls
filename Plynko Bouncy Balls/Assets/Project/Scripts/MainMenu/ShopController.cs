using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{

    public void BuyAddBall()
    {
        GameManager.Instance.BuyAddBall();
    }

    public void JoinBalls()
    {
        GameManager.Instance.JoinBalls();
    }

    public void BuyUniversalBall()
    {
        GameManager.Instance.BuyUniversalBall();
    }

    public void BuyBallUpdate()
    {
        GameManager.Instance.UpgradeAddBall();
    }

    public void BuyJointBallUpdate()
    {
        GameManager.Instance.UpgradeJoinBall();
    }

    public void BuyMultiBallUpdate()
    {
        GameManager.Instance.UpgradeMultiBall();
    }
}