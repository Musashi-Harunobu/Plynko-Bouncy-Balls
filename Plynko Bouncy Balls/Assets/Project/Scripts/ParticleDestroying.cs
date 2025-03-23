using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroying : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    
    

    public IEnumerator DestroyParticles()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
