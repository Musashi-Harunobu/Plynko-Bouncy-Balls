using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour
{
    [SerializeField] private UnityEvent onPauseEnable;
    [SerializeField] private UnityEvent onPauseDisable;
    
    
    
    public void OnPause()
    {
        Time.timeScale = 0;
        onPauseEnable.Invoke();
    }

    public void OnUnpause()
    {
        Time.timeScale = 1;
        onPauseDisable.Invoke();
    }
}
