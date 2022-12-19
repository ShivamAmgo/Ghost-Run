using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class VIctimThrownMovement : MonoBehaviour
{
     private List<Transform> BodyHitGroundPoints;
    [SerializeField] private float ThrownPower = 5;
    [SerializeField] private float thrownDuration = 2;
    [SerializeField] private Rigidbody FixedJointHead;
    [SerializeField] private float SlowMoDuration = 1;
    //[SerializeField] private float slowMotionTimeScale = 0.5f;


    private int FallCount = 0;

    private void OnEnable()
    {
        EndLine.OnBodyFallpositionsDeleiver += StoreBodyFallpositions;
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
            SlowMotionEffect();
            CheckBodyFallPositions();
        });
    }
    void SlowMotionEffect()
    {
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
        
    }
    public void Thrown()
    {
        transform.parent = null;
        if (FixedJointHead!=null)
        {
            FixedJointHead.isKinematic = false;
        }
        ThrowBody(BodyHitGroundPoints[0],ThrownPower,thrownDuration);
    
    }
    /*
    public void StartSlowMotion()
    {

        
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = _startFixedDeltaTime * slowMotionTimeScale;
        StopSlowMotionAfterDelay();
       
    }

    

    public void StopSlowMotion()
    {
        Time.timeScale = _startTimeScale;
        Time.fixedDeltaTime = _startFixedDeltaTime;
        

    }
    */
    private void OnDisable()
    {
        EndLine.OnBodyFallpositionsDeleiver -= StoreBodyFallpositions;
    }
}
