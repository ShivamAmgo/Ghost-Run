using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{
    [SerializeField] private List<Ghost> AllGhosts;
    [SerializeField] private ParticleSystem TransformationFx;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField]float SpeedIncreaseFactor=1;
    [SerializeField] private int ParticleCount = 50;
    private int ActiveGhostIndex = 0;
    
    private void OnEnable()
    {
        GameManagerGhost.TransformGhostPlayer += TransformGhost;
        AllGhosts[0].gameObject.SetActive(true);
    }

    void TransformGhost(bool Transformstatus)
    {
        if (AllGhosts.Count-1==ActiveGhostIndex)
        {
            return;
        }

       
        TransformationFx.Emit(ParticleCount);
        AllGhosts[ActiveGhostIndex].gameObject.SetActive(false);
        AllGhosts[++ActiveGhostIndex].gameObject.SetActive(true);
        AllGhosts[ActiveGhostIndex].Transformed();
        m_PlayerMovement.Speed += SpeedIncreaseFactor;
        if (AllGhosts.Count-1==ActiveGhostIndex)
        {
            AllGhosts[ActiveGhostIndex].ActiveAttackTrigger();
        }
    }

    private void OnDisable()
    {
        GameManagerGhost.TransformGhostPlayer -= TransformGhost;
    }
}
