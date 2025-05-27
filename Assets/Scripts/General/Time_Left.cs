using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_Left : MonoBehaviour
{
    public AudioSource m_AudioSource;
    public Slider TimeSlider;
    void Update()
    {
        TimeSlider.value = m_AudioSource.time;
    }
}
