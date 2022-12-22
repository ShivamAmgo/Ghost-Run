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
   [SerializeField] private bool ObstacleForPlayer = true;
   [SerializeField] private bool Randomness = true;
   [SerializeField] private Vector3 direction = new Vector3(0, 0, 1);
   public delegate void JumpTriggered(float JumpHeightParam);
    public delegate void ObstacleHit();
    public static event ObstacleHit OnObstacleHit;
   public static event JumpTriggered OnJumpTriggered;
   private void OnTriggerEnter(Collider other)
   {
      if (other.tag=="Victim" && !Used)
      {
          
          Used = true;
          if (ObstacleForPlayer)
          {
              OnJumpTriggered?.Invoke(JumpHeight);
          }
          else
          {
              Shattered = true;
              Shatter();
          }
          
      }

      else if (other.tag=="Player" && !Shattered)
      {
          OnObstacleHit?.Invoke();
          Shattered = true;
          Shatter();
      }
   }

   void Shatter()
   {
       int count = 0;
       //Debug.Log("colliede "+transform.name);
       foreach (var rb in rigidbodies)
       {
           //var direction = rb.transform.forward;
           float randomness = 1;
           rb.isKinematic = false;
           if (count%2==0)
           {
               randomness = -1;
           }

           if (Randomness)
           {
               rb.AddForce(direction * (throwBackForce)*randomness + Vector3.up*upForce, ForceMode.Impulse);
           }
           else
           {
               rb.AddForce(direction * (throwBackForce) + Vector3.up*upForce, ForceMode.Impulse);
           }
           
           rb.AddTorque(6,3,10);
           count++;
       }
       StartCoroutine(Destroy());
       if ( PartsToDisable.Length<1)
       {
           return;
       }
       foreach (GameObject obj in PartsToDisable)
       {
           obj.SetActive(false);
       }

       
   }

   IEnumerator Destroy()
   {
       yield return new WaitForSeconds(4);
       Destroy(gameObject);
   }
}
