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
    private int ActiveGhostIndex = 0;

    private void OnEnable()
    {
        GameManagerGhost.TransformGhostPlayer += TransformGhost;
    }

    void TransformGhost()
    {
        if (AllGhosts.Count-1==ActiveGhostIndex)
        {
            return;
        }
        TransformationFx.Emit(30);
        AllGhosts[ActiveGhostIndex].gameObject.SetActive(false);
        AllGhosts[++ActiveGhostIndex].gameObject.SetActive(true);
        m_PlayerMovement.Speed += SpeedIncreaseFactor;
    }

    private void OnDisable()
    {
        GameManagerGhost.TransformGhostPlayer -= TransformGhost;
    }
}
