using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
   public delegate void MirrorBreak();

   public static event MirrorBreak OnMirrorBreak;

   public void OnBreakMirror()
   {
      OnMirrorBreak?.Invoke();
   }
}
