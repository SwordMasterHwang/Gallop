using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSound : MonoBehaviour
{
    private AudioSource AudioSource;
    void Start()
    {
        AudioSource = this.gameObject.GetComponent<AudioSource>();
    }

  
    public void OnPlaySound()
    {
        AudioSource.Play();
    }
    
    
}
