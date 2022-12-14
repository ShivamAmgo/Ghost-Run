using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullRotation : MonoBehaviour
{
    private float Timer = 0;
    private void FixedUpdate()
    {
        if (transform.parent==null)
        {
            Timer += Time.deltaTime * 100;
            transform.eulerAngles = new Vector3(0, Timer, 0);
        }
    }
}
