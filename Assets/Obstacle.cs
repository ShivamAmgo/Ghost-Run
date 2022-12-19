using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
   private bool Used = false;
   private bool Shattered = false;
   [SerializeField] private float JumpHeight = 4f;
   [SerializeField] private Rigidbody[] rigidbodies;
   [SerializeField] private float throwBackForce = 5;
   [SerializeField] private float upForce = 3;
   [SerializeField] private GameObject[] PartsToDisable;
   public delegate void JumpTriggered(float JumpHeightParam);

   public static event JumpTriggered OnJumpTriggered;
   private void OnTriggerEnter(Collider other)
   {
      if (other.tag=="Victim" && !Used)
      {
          
          Used = true;
          OnJumpTriggered?.Invoke(JumpHeight);
      }

      if (other.tag=="Player" && !Shattered)
      {
          Shattered = true;
          Shatter();
      }
   }

   void Shatter()
   {
       int count = 0;
       var direction = -transform.forward;
       foreach (var rb in rigidbodies)
       {
           float randomness = 1;
           rb.isKinematic = false;
           if (count%2==0)
           {
               randomness = -1;
           }
           rb.AddForce(direction * (throwBackForce)*randomness + Vector3.up*upForce, ForceMode.Impulse);
           count++;
       }

       if ( PartsToDisable.Length<1)
       {
           return;
       }
       foreach (GameObject obj in PartsToDisable)
       {
           obj.SetActive(false);
       }

       StartCoroutine(Destroy());
   }

   IEnumerator Destroy()
   {
       yield return new WaitForSeconds(4);
       Destroy(gameObject);
   }
}
