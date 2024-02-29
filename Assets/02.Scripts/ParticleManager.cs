using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    
    public void Explosion()
    {
        ParticleSystem.Play();
    }
}
