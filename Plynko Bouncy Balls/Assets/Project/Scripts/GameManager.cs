using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [HideInInspector] public int Starts = 0;
    [HideInInspector] public int GameBalls = 1;
    
}
