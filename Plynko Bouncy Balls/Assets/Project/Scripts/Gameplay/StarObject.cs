using UnityEngine;

public class StarObject : MonoBehaviour
{
    [SerializeField] private int starValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.GetComponent<Ball>();
        if (ball != null)
        {
            GameManager.Instance.AddStars(starValue);

            Destroy(gameObject);
        }
    }
}