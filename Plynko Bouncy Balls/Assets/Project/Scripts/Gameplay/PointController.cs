using TMPro;
using UnityEngine;

public class PointController : MonoBehaviour
{
    [SerializeField] private GameManager.BallType pointColor; 
    [SerializeField] private TextMeshPro strengthText;

    private int _strength;

    private void Awake()
    {
        _strength = Random.Range(4, 8);
    }

    private void Update()
    {
        strengthText.text = _strength.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            if (ball.ballType == GameManager.BallType.Green)
            {
                _strength -= 2;
            }
            else if (ball.ballType == pointColor)
            {
                _strength -= 2;
            }
            else
            {
                _strength -= 1;
            }

            if (_strength <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}