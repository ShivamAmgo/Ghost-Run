using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesMaterialController : MonoBehaviour
{
   [SerializeField] private Material MaterialToApply;
   [SerializeField] private Transform TargetObj;
   private MeshRenderer[] AllTreesMesh;

   private void Start()
   {
      AllTreesMesh = GetComponentsInChildren<MeshRenderer>();
      SetMaterial();
   }

   void SetMaterial()
   {
      foreach (MeshRenderer MR in AllTreesMesh)
      {
         MR.material = MaterialToApply;
      }
   }
}
