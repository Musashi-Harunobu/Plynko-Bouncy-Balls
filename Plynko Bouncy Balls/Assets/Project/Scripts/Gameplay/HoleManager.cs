using UnityEngine;

public class HoleManager : MonoBehaviour
{
    [SerializeField] private bool isBigHole;
    [SerializeField] private int holeValue = 10;

    private ScaleManager _scaleManager;

    private void Awake()
    {
        _scaleManager = GetComponent<ScaleManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Ball ballComponent = other.gameObject.GetComponent<Ball>();
        if (ballComponent != null)
        {
            if (isBigHole)
            {
                // Большая лунка – мяч возвращается
                var ballType = ballComponent.ballType;
                Destroy(other.gameObject);
                GameManager.Instance.roundBalls.Add(ballType);

                _scaleManager.ChangeScale();

                GameManager.Instance.OnBallFinished();
            }
            else
            {
                Destroy(other.gameObject);

                GameManager.Instance.CurrentGameScore += holeValue;
                GameManager.Instance.AddStars(holeValue);

                _scaleManager.ChangeScale();

                GameManager.Instance.OnBallFinished();
            }
        }
    }


}