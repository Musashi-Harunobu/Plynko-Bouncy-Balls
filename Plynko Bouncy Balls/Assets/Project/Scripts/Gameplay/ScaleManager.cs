using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour
{
    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Ball>())
        {
            StartCoroutine(ScaleChanger());
        }
    }

    public void ChangeScale()
    {
        StartCoroutine(ScaleChanger());
    }

    private IEnumerator ScaleChanger()
    {
        float duration = 0.5f; 
        float halfwayDuration = duration / 2; 
        float elapsed = 0f;
        Vector3 targetScale = _originalScale * 1.2f; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (elapsed < halfwayDuration)
            {
                float t = elapsed / halfwayDuration;
                transform.localScale = Vector3.Lerp(_originalScale, targetScale, t);
            }
            else
            {
                float t = (elapsed - halfwayDuration) / halfwayDuration;
                transform.localScale = Vector3.Lerp(targetScale, _originalScale, t);
            }

            yield return null; 
        }

        transform.localScale = _originalScale;
    }
}
