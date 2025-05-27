using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is important for optimisation, since moving objects on the ui is pretty bad, but is passable when only displaying the ui
/// </summary>
public class DisableAudioVisualiserUI : MonoBehaviour
{
    public AudioSource chosenAudio;
    public GameObject audioVisualisers;

    private void Start()
    {
        audioVisualisers.SetActive(false);
    }

    void Update()
    {
        if (chosenAudio.isPlaying)
        {
            audioVisualisers.SetActive(true);
        }
        else
        {
            audioVisualisers.SetActive(false);
        }
    }
}
