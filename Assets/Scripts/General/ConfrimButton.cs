using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    public Canvas StartMenu;
    public Canvas OptionsMenu;
    public Canvas ChooseAudioMenu;
    public Canvas ModifyRhythmGameMenu;

    public AudioSource chosenAudio;
    public GameObject confirmButton;

    private void Start()
    {
        confirmButton.SetActive(false);
    }

    void Update()
    {
        if (chosenAudio.isPlaying)
        {
            confirmButton.SetActive(true);
        }
        else
        {
            confirmButton.SetActive(false);
        }
    }
}
