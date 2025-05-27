using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSong : MonoBehaviour
{
    public AudioSource RythmAudio;
    private bool isPlaying = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note") && !isPlaying)
        {
            //RythmAudio.Play();
            isPlaying = true;
        }
    }
}
