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
     float TransformCount = 0;
    [SerializeField] private float TotalGhosts = 4;
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

        Physics.gravity = new Vector3(0, -18f, 0);
    }
    
    public void AddSkullPoint()
    {
        
        SkullPoints++;
        if (SkullPoints>=SkullsToTransform)
        {
            SkullPoints = 0;
            TransformCount++;
            
            if (TransformCount>=TotalGhosts)
            {
                Debug.Log("Limit Reached");
                return;
            }
            TransformGhostPlayer?.Invoke(true);
            
        }
    }
}
