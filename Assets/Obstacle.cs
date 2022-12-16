using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
   private bool Used = false;
   [SerializeField] private float JumpHeight = 0.5f;
   public delegate void JumpTriggered(float JumpHeightParam);

   public static event JumpTriggered OnJumpTriggered;
   private void OnTriggerEnter(Collider other)
   {
      if (other.tag=="Victim" && !Used)
      {
          
          Used = true;
          OnJumpTriggered?.Invoke(JumpHeight);
      }
   }
}
