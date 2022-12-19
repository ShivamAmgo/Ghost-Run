using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicHandAttack : MonoBehaviour
{
    public delegate void VictimCathed(Transform Victim);

    public static event VictimCathed OnVictimCatched;
    private bool used = false;
    [SerializeField] private Vector3 VictimCaughtPosAlign;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Victim" && !used)
        {
            used = true;
            OnVictimCatched?.Invoke(other.transform.root);
            CatchVictim(other.transform.root);
        }
    }

    void CatchVictim(Transform Victim)
    {
        Victim.parent = transform;
        Victim.localPosition = VictimCaughtPosAlign;
    }
}
