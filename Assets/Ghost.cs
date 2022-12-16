using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator m_Animator;
    [SerializeField] private PlayerMovement m_PlayerMovement;

    private void OnEnable()
    {
        CameraFollow.OnCameraFocusGhost += CrawlOut;
    }

    private void CrawlOut()
    {
        m_Animator.enabled = true;
    }

    public void SetRootMotion(int status)
    {
        //return;
        if (status==0)
        {
            m_Animator.applyRootMotion = false;
            m_Animator.SetTrigger("Run");
            transform.root.position = transform.position;
            transform.localPosition=Vector3.zero;
            m_PlayerMovement.isMoving = true;
            
        }
        else
        {
            m_Animator.applyRootMotion = true;
        }
        
    }

    private void OnDisable()
    {
        CameraFollow.OnCameraFocusGhost -= CrawlOut;
    }
}
