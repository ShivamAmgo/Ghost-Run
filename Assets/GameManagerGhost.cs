using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerGhost : MonoBehaviour
{
    public static GameManagerGhost Instance{ get; private set; }
    public  delegate void TransformGhost(bool TransformStatus);

    public static event TransformGhost TransformGhostPlayer;
    private int SkullPoints = 0;

    [SerializeField]int SkullsToTransform;
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    public void AddSkullPoint()
    {
        
        SkullPoints++;
        if (SkullPoints>=SkullsToTransform)
        {
            SkullPoints = 0;
            TransformGhostPlayer?.Invoke(true);
        }
    }
}
