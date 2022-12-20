using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator m_Animator;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private float AttackDistane = 2;
    [SerializeField] private VictimMovement m_VictimMovement;

    [SerializeField] Transform CameraposAtSoulSuckPoint;
    private bool VictimChased = false;
    
    private void OnEnable()
    {
        CameraFollow.OnCameraFocusGhost += CrawlOut;
        MimicHandAttack.OnVictimCatched += VictimCatched;
        EndLine.OnFinish += OnFinishLine;
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

    private void Update()
    {
        if (m_VictimMovement.transform.position.z-transform.position.z<=AttackDistane && !VictimChased)
        {
            VictimChased = true;
            AttackVictim();
        }
    }

    void AttackVictim()
    {
        m_PlayerMovement.Speed = m_VictimMovement.Speed;
        //Debug.Log("name "+transform.name);
        if (GetComponentInChildren<MimicHandAttack>()!=null)
        {
            m_Animator.SetTrigger("Attack");
        }
        
    }

    public void VictimCatched(Transform Victim)
    {
        m_Animator.SetTrigger("Idle");
    }
    private void OnFinishLine()
    {
        m_PlayerMovement.Speed = 0;
        CameraFollow.Instance.FocusOnVictim(CameraposAtSoulSuckPoint,null);
        m_Animator.SetTrigger("SoulSuck");
    }

    public void Throw()
    {
        VIctimThrownMovement VTM = GetComponentInChildren<VIctimThrownMovement>();
        if (VTM==null)
        {
            return;
        }
        VTM.Thrown();
        m_PlayerMovement.enabled = false;
    }
    private void OnDisable()
    {
        CameraFollow.OnCameraFocusGhost -= CrawlOut;
        MimicHandAttack.OnVictimCatched -= VictimCatched;
        EndLine.OnFinish -= OnFinishLine;
    }
}
