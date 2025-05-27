using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiatePlayerAudio : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioSource AnalysisAudio;
    public AudioSource RhythmAudio;
    public Canvas ChooseAudio;
    public Canvas ModifyRhythmGame;

    private void Start()
    {
        ChooseAudio.enabled = true;
        ModifyRhythmGame.enabled = false;
        //DontDestroyOnLoad(gameObject);        
    }
    public void OnConfirmation()
    {
        AnalysisAudio.clip = playerAudio.clip;
        RhythmAudio.clip = playerAudio.clip;
        playerAudio.Pause();

        AnalysisAudio.Play();
        RhythmAudio.Play();

        ChooseAudio.enabled = false;
        ModifyRhythmGame.enabled = true;
    }
    public void OnDeconfirmation()
    {
        AnalysisAudio.clip = null;
        RhythmAudio.clip = null;
        playerAudio.UnPause();

        AnalysisAudio.Pause();
        RhythmAudio.Pause();

        ChooseAudio.enabled = true;
        ModifyRhythmGame.enabled = false;
    }
}
