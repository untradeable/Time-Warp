using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Audio : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }
}