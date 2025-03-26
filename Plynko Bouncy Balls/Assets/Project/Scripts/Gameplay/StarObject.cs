using UnityEngine;

public class StarObject : MonoBehaviour
{
    [SerializeField] private int starValue = 1;
    [SerializeField] private ParticleSystem starParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.GetComponent<Ball>();
        if (ball != null)
        {
            GameManager.Instance.AddStars(starValue);
            StarSpawner.CollectedStars++;

            var ps = Instantiate(starParticle, transform.position, Quaternion.identity);
            ps.Play();
            Destroy(gameObject);
        }
    }
}