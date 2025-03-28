using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public GameManager.BallType ballType;
    
    [SerializeField] private ParticleSystem particle;
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<HoleManager>())
        {
            var ps = Instantiate(particle, transform.position, Quaternion.identity);
            ps.Play();
        }
        Debug.Log($"Ball collided with {other.gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"Ball trigger with {col.gameObject.name}");
    }

    private void OnDestroy()
    {
        GameManager.Instance.SetBallInFlight(false);
    }
}