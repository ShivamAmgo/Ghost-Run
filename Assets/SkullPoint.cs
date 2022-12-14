using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPoint : MonoBehaviour
{
    private bool Used = false;
    [SerializeField] private ParticleSystem DestroyFX;
    [SerializeField] private GameObject MeshSkull;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player" && !Used)
        {
            Used = true;
            GameManagerGhost.Instance.AddSkullPoint();
            StartCoroutine(DestroySkull());
        }
    }

    IEnumerator DestroySkull()
    {
        MeshSkull.SetActive(false);
        DestroyFX.Play();
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
