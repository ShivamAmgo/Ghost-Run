using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLine : MonoBehaviour
{
   public List<Transform> BodyFallPos;

   public delegate void DeliverVictimFallpositions(List<Transform> BodyFallpos);

   public delegate void Finished();

   public static event Finished OnFinish;
   
   public static event DeliverVictimFallpositions OnBodyFallpositionsDeleiver;

   private void Start()
   {
      OnBodyFallpositionsDeleiver?.Invoke(BodyFallPos);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag=="Player")
      {
         OnFinish?.Invoke();
      }
   }
}
