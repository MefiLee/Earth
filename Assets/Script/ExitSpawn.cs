using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSpawn : MonoBehaviour
{
    private AudioSource audio;

    public AudioClip SpawnExitSound;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = SpawnExitSound;
        audio.Play();
    }

    
}
