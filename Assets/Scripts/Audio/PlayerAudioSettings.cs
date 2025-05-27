using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerAudioSettings : MonoBehaviour
{
    public MoveDown MoveDownGreen;
    public MoveDown MoveDownBlue;
    public MoveDown MoveDownRed;
    public MoveDown MoveDownPink;

    public AudioPeer AudioPeer;

    public AudioSource audioSource;

    public Slider SpeedSlider;
    public Slider SensitivitySlider;
    public Slider TimeBetweenNotes;
    public Slider VolumeSlider;

    private bool SettingsConfirmed;
    public float ConfirmedSpeed;
    public float ConfirmedSensitivity;
    public float ConfirmedTimeBetweenNotes;
    public float ConfirmedVolume;   

    void Update()
    {
        if (SettingsConfirmed == false)
        {
            MoveDownGreen.speed = SpeedSlider.value;
            MoveDownBlue.speed = SpeedSlider.value;
            MoveDownRed.speed = SpeedSlider.value;
            MoveDownPink.speed = SpeedSlider.value;

            AudioPeer.minDifference = SensitivitySlider.value;

            AudioPeer.timeBetweenBeats = TimeBetweenNotes.value;

            audioSource.volume = VolumeSlider.value;
        }
    }

    // For optimisation and prevent settings potentially being changed mid song
    public void GameStarted()
    {
        SettingsConfirmed = true;
        ConfirmedSpeed = SpeedSlider.value;
        ConfirmedSensitivity = SensitivitySlider.value;
        ConfirmedTimeBetweenNotes = TimeBetweenNotes.value;
        ConfirmedVolume = VolumeSlider.value;

        MoveDownGreen.speed = ConfirmedSpeed;
        MoveDownBlue.speed = ConfirmedSpeed;
        MoveDownRed.speed = ConfirmedSpeed;
        MoveDownPink.speed = ConfirmedSpeed;

        AudioPeer.minDifference = ConfirmedSensitivity;

        AudioPeer.timeBetweenBeats = ConfirmedTimeBetweenNotes;

        audioSource.volume = ConfirmedVolume;
    }
}
