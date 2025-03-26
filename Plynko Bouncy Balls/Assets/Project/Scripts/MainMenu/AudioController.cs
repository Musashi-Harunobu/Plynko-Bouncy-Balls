using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private GameObject SoundOnSprite;
    [SerializeField] private GameObject SoundOffSprite;

    private void FixedUpdate()
    {
        if (GameManager.Instance.AudioToggle)
        {
            SoundOnSprite.SetActive(false);
            SoundOffSprite.SetActive(true);
        }
        else if (!GameManager.Instance.AudioToggle)
        {
            SoundOnSprite.SetActive(true);
            SoundOffSprite.SetActive(false);
        }
    }

    public void AudioControl()
    {
        GameManager.Instance.AudioSourceToggle();
    }
}
