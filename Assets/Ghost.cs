using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator m_Animator;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private float AttackDistane = 2;
    [SerializeField] private VictimMovement m_VictimMovement;
    [SerializeField]ParticleSystem[] SoulSucksFX;
    [SerializeField] Transform CameraposAtSoulSuckPoint;
    [SerializeField] private SphereCollider Attacktrigger;
    [SerializeField] private GameObject[] ghost;
    [SerializeField] private SphereCollider[] MirrorDetectors;
    private bool CrawledOut = false;

    public delegate void Chasing();

    public static event Chasing OnGhostChasing;
    
    private bool VictimChased = false;
    
    private void OnEnable()
    {
        CameraFollow.OnCameraFocusGhost += CrawlOut;
        MimicHandAttack.OnVictimCatched += VictimCatched;
        EndLine.OnFinish += OnFinishLine;
        //m_Animator.enabled = true;
       ActivateGhost(false);
        
    }

   


    private void CrawlOut()
    {
        m_Animator.SetTrigger("CrawlOut");
        m_Animator.enabled = true;
        
        ActivateGhost(true);
        DisableDetectors(false);
    }

    void ActivateGhost(bool ActiveStatus)
    {
        foreach (var obj in ghost)
        {
            obj.SetActive(ActiveStatus);
        }
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
            transform.localRotation=Quaternion.identity;
            m_PlayerMovement.isMoving = true;
            OnGhostChasing?.Invoke();
            
        }
        else
        {
            m_Animator.applyRootMotion = true;
        }
        
    }

    public void EmergeOutFlyingFromWell()
    {
        Debug.Log("Flying");
        if (CrawledOut)
        {
            return;
        }

        CrawledOut = true;
        transform.DOJump(transform.position + new Vector3(0, 0, 7), 3.75f, 0,2.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            DOVirtual.DelayedCall(1, () =>
            {
                SetRootMotion(0);
            });
            
        });
    }
    public void Transformed()
    {
        m_Animator.enabled = true;
        m_Animator.applyRootMotion = false;
        m_Animator.SetTrigger("Run");
        ActivateGhost(true);
        DisableDetectors(true);
        
    }

    public void DisableDetectors(bool Status)
    {
        foreach (var VARIABLE in MirrorDetectors)
        {
            VARIABLE.enabled = !Status;
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
        m_PlayerMovement.Speed = m_VictimMovement.MaxSpeed;
        //Debug.Log("name "+transform.name);
        if (GetComponentInChildren<MimicHandAttack>()!=null)
        {
            m_Animator.SetTrigger("Attack");
        }
        
    }

    public void ActiveAttackTrigger()
    {
        Attacktrigger.enabled = true;
    }
    public void VictimCatched(Transform Victim)
    {
        m_Animator.SetTrigger("Idle");
    }
    private void OnFinishLine()
    {
        m_PlayerMovement.enabled = false;
        CameraFollow.Instance.FocusOnVictim(CameraposAtSoulSuckPoint,null);
        if(GetComponentInChildren<VIctimThrownMovement>()==null)
        {
            //Lose Panel
            GameManagerGhost.Instance.SetWin(false);
            m_Animator.SetTrigger("Scream");
            this.enabled = false;
            return;
        }
        m_Animator.SetTrigger("SoulSuck");
        
    }
    public void PlaySoulEffects()
    {
        foreach(ParticleSystem ps in SoulSucksFX)
        {
            ps.Play();
        }
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
