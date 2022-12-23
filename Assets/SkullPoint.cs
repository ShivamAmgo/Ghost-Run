using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPoint : MonoBehaviour
{
    private bool Used = false;
    [SerializeField] private ParticleSystem DestroyFX;
    [SerializeField] private GameObject MeshSkull;
    [SerializeField] private bool IsBible = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player" && !Used)
        {
            Used = true;
            if (IsBible)
            {
                GameManagerGhost.Instance.AddSkullPoint(-1);
            }
            else
            {
                GameManagerGhost.Instance.AddSkullPoint(1);
            }
            
            StartCoroutine(DestroyObj());
        }
    }

    IEnumerator DestroyObj()
    {
        MeshSkull.SetActive(false);
        DestroyFX.Play();
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
