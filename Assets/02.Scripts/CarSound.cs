using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    private AudioSource AudioSource;
    public AudioClip[] AudioClips;
    public GameObject sucess;
    public GameObject fail;
    private void Awake()
    {
        AudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        //완주성공이나 완주실패시에는 사운드 종료
        if (sucess.activeSelf == true)
        {
            AudioSource.Stop();
        }
        else if (fail.activeSelf == true)
        {
            AudioSource.Stop();
        }
    }

    public void OnVertical()
    {
        AudioSource.clip = AudioClips[0];
        AudioSource.loop = true;
        AudioSource.Play();
        AudioSource.PlayOneShot(AudioClips[2]);
    }
    
    public void OnBreak()
    {
        AudioSource.Stop();
        AudioSource.clip = AudioClips[1];
        AudioSource.Play();
        AudioSource.loop = false;
    }

    public void OnDrift()
    {
        AudioSource.Stop();
        AudioSource.clip = AudioClips[3];
        AudioSource.Play();
        AudioSource.loop = false;
    }

    public void OnCrush()
    {
        AudioSource.Stop();
        AudioSource.clip = AudioClips[5];
        AudioSource.Play();
        AudioSource.loop = false;
    }

    public void OnExplosion()
    {
        AudioSource.Stop();
        AudioSource.clip = AudioClips[4];
        AudioSource.PlayOneShot(AudioSource.clip);
        AudioSource.loop = false;
    }
}
