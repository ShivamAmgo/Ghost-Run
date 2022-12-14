using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public bool CinematicCamera = false;
   [SerializeField] private bool follow = false;
   [SerializeField] private float Rotate_inDegrees = 60;
   [SerializeField] private Transform ChildCamera;
    public Transform Target;
   [SerializeField] private Transform SourceCamera;
   [SerializeField] private Transform CinematicPoint;
   [SerializeField] private Vector3 offset;
   [SerializeField] private Vector3 NormalOffsetFromtransform;
   [SerializeField] private float RotateSpeed = 2;
   [SerializeField] private float RotateLimit = 60;
   [SerializeField] private Vector3 WinCamOffset;
   private Vector3 cameraposAfterCalculation = Vector3.zero;
/*
   private void OnEnable()
   {
      StuntController.OnCinematicCameraPlay += CheckCameraMode;
      EdgeFall.OnEdgeDetected += StopFollowing;
      StuntController.OnWinningDance += WinCam;
      EnemyBoss.OnFlyingPunch += FollowEnemyBoss;
      PlayerMovement.DeliverPlayerInfo += ReceivePlayer;
      //PlayerFall.
      
   }
*/
   private void ReceivePlayer(Transform player)
   {
      Target = player;
   }

   private void WinCam()
   {
      follow = false;
      NormalOffsetFromtransform = WinCamOffset;
      transform.DOMove(Target.position - WinCamOffset, 1);
   }

   private void Start()
   {
      cameraposAfterCalculation = offset;
      
   }

   private void Update()
   {

      if (follow)
      {
         
         transform.position = Target.position - NormalOffsetFromtransform;
      }

      if (CinematicCamera)
      {
         if (ChildCamera.eulerAngles.y >= RotateLimit)
         {
            CinematicCamera = false;
            return;
         }

         ChildCamera.RotateAround(CinematicPoint.position, Vector3.up, Rotate_inDegrees * Time.deltaTime * RotateSpeed);
      }

   }

   void CheckCameraMode(bool Status)
   {
      if (Status)
      {
         CinematicCamera = true;
      }
      else if (!Status)
      {
         RotateCameraToOriginalPos();
      }

   }

   void RotateCameraToOriginalPos()
   {
      ChildCamera.DOLocalMove(Vector3.zero, 1f);
      ChildCamera.DOLocalRotate(Vector3.zero, 1f);
   }
/*
   void StopFollowing(EdgeFall edgeFall)
   {
      if (edgeFall.PlayerWillDie)
      {
         follow = false;
         CinematicCamera = true;
      }
   }*/

   void FollowEnemyBoss(Transform FollowTarget)
   {
      if (FollowTarget.tag=="Player")
      {
         follow = false;
         Target = FollowTarget;
         WinCam();
      }
      Target = FollowTarget;
   }
/*
   private void OnDisable()
   {
      StuntController.OnCinematicCameraPlay -= CheckCameraMode;
      EdgeFall.OnEdgeDetected -= StopFollowing;
      StuntController.OnWinningDance -= WinCam;
      EnemyBoss.OnFlyingPunch -= FollowEnemyBoss;
      PlayerMovement.DeliverPlayerInfo -= ReceivePlayer;
   }*/
}
