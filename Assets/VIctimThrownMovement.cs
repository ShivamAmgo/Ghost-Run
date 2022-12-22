using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class VIctimThrownMovement : MonoBehaviour
{
     private List<Transform> BodyHitGroundPoints;
    [SerializeField] private float ThrownPower = 5;
    [SerializeField] private float thrownDuration = 2;
    [SerializeField] private Rigidbody FixedJointHead;
    //[SerializeField] private float SlowMoDuration = 1;

    [SerializeField] private VictimMovement m_VictimMovement;

    [SerializeField] private Quaternion ThrownRot;

    [SerializeField] private Transform FreeFallPartRotate;
    //[SerializeField] private float slowMotionTimeScale = 0.5f;
    private Transform GhostPlayer;

    private int FallCount = 0;

   
    private void OnEnable()
    {
        EndLine.OnBodyFallpositionsDeleiver += StoreBodyFallpositions;
        PlayerMovement.DeliverPlayerInfo += GetIncomingPlayer;
    }

    private void GetIncomingPlayer(Transform player)
    {
        GhostPlayer = player;
    }

    private void Start()
    {
        
    }
    private void StoreBodyFallpositions(List<Transform> bodypos)
    {
        BodyHitGroundPoints = bodypos;
    }
/*
    void Thrown(List<Transform> BodyfallPos)
    {
        transform.DOJump(BodyfallPos[0].position, ThrownPower, 1, thrownDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            SlowMotionEffect();
           
            
        });
    }
    */

    void CheckBodyFallPositions()
    {
        if (FallCount==BodyHitGroundPoints.Count)
        {
            FixedJointHead.isKinematic = false;
            DOVirtual.DelayedCall(4, () =>
            {
                CameraFollow.Instance.PlayCinematicMode(false);
                GameManagerGhost.Instance.SetCameraToFollow(GhostPlayer);
                GameManagerGhost.Instance.SetWin(true);
            });
            return;
        }
        else
        {
            thrownDuration -=0.2f;
            ThrownPower -= 0.5f;
            ThrowBody(BodyHitGroundPoints[FallCount],ThrownPower,thrownDuration);
        }
    }
    void ThrowBody(Transform POS,float ThrownPower,float thrownDuration)
    {
        
        transform.DOJump(POS.position, ThrownPower, 1, thrownDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            FallCount++;
            if (FallCount<BodyHitGroundPoints.Count)
            {
                RotateVictimFreeFall();
                SlowMotionEffect();
            }
            
            CheckBodyFallPositions();
        });
    }

    void RotateVictimFreeFall()
    {
        transform.DORotate(new Vector3(transform.eulerAngles.x+Random.Range(60,360),0,0), (thrownDuration-0.1f), RotateMode.Fast);
    }
    void SlowMotionEffect()
    {
        Time.timeScale = 0.25f;
        DOVirtual.DelayedCall(1, () =>
        {
            Time.timeScale = 1;
        });
        /*
        float c = Time.timeScale;
        DOTween.To(() => c, x => c = x, 0.2f, SlowMoDuration).OnUpdate(() =>
        {
            Time.timeScale = c;
        }).OnComplete(() =>
        {
            DOTween.To(() => c, x => c = x, 1f, SlowMoDuration).OnUpdate(() =>
            {
                Time.timeScale = c;
            });
        });
        */


    }
    public void Thrown()
    {
        transform.parent = null;
        m_VictimMovement.DisableColliders(false);
        GameManagerGhost.Instance.SetCameraToFollow(transform);
        CameraFollow.Instance.PlayCinematicMode(true);
        if (FixedJointHead!=null)
        {
            //FixedJointHead.isKinematic = false;
        }
        ThrowBody(BodyHitGroundPoints[0],ThrownPower,thrownDuration);
        transform.rotation = ThrownRot;
    }
   
    private void OnDisable()
    {
        EndLine.OnBodyFallpositionsDeleiver -= StoreBodyFallpositions;
        PlayerMovement.DeliverPlayerInfo -= GetIncomingPlayer;
    }
}
