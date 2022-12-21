using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerGhost : MonoBehaviour
{
    public static GameManagerGhost Instance{ get; private set; }
    public  delegate void TransformGhost(bool TransformStatus);

    public delegate void SetCameraFollow(Transform target);

    public static event SetCameraFollow OnSettingCameraToFollow;
    public static event TransformGhost TransformGhostPlayer;
    private int SkullPoints = 0;

	

    [SerializeField]int SkullsToTransform;
     float TransformCount = 0;
    [SerializeField] private float TotalGhosts = 4;
    [SerializeField]GameObject[] WinAndLosePanel;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }
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
    public void SetWin(bool WinStatus)
    {
        if(WinStatus)
        {
            WinAndLosePanel[0].SetActive(true);
        }
        else
        {
            WinAndLosePanel[1].SetActive(true);
        }
        
    }
    public void SetCameraToFollow(Transform Target)
    {
        if (Target==null)
        {
            
        }
        OnSettingCameraToFollow(Target);
    }
}
