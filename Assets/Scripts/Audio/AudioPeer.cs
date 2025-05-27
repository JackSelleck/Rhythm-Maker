using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour /// this script analyses the audio for note generation and audio visualising
{
    public DetectPeaks detectPeaks;
    public AudioSource _audioSource;

    /// Audio Visualising
    [HideInInspector] public float[] _samples = new float[2048];
    [HideInInspector] public float[] _freqBand = new float[4];
    [HideInInspector] public float[] _bandBuffer = new float[4];
    [HideInInspector] public float[] _bufferDecrease = new float[4];

    public float timeBetweenBeats = 0.5f;
    public float minDifference = 0.02f;

    // Audio peak findings
    /// Green Notes
    private bool canSpawnGreenNote = true;
    private float _greenLaneHertz;
    private float greenCurSpectrum;
    private float greenPrevSpectrum;
    /// Blue Notes
    private bool canSpawnBlueNote = true;
    private float _blueLaneHertz;
    private float blueCurSpectrum;
    private float bluePrevSpectrum;
    /// Red Notes
    private bool canSpawnRedNote = true;
    private float _redLaneHertz;
    private float redCurSpectrum;
    private float redPrevSpectrum;
    /// Pink Notes
    private bool canSpawnPinkNote = true;
    private float _pinkLaneHertz;
    private float pinkCurSpectrum;
    private float pinkPrevSpectrum;
    /// Long Notes
    public float longNoteSensitivity;
    List<float> NoteLength = new List<float>();
    public float longNoteGreen;
    /// Events to be called in DetectPeaks.cs
    private bool canSpawnRandomNote = true;
    public GreenBeatEventHandler GreenBeat;
    public BlueBeatEventHandler BlueBeat;
    public RedBeatEventHandler RedBeat; 
    public PinkBeatEventHandler PinkBeat;
    public RandomLaneEventHandler RandomBeat;

    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Audio Analysis
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
                     
        // Beat Analysis
        GreenAudioPeaks();
        BlueAudioPeaks();
        RedAudioPeaks();
        PinkAudioPeaks();
    }
    void GetSpectrumAudioSource()
    {
        // Gets the hertz values of the current song
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.BlackmanHarris);
    }

    void BandBuffer()
    {
        // Buffers the audio visualiser to have less erratic movement
        // Counts what sample should be buffered, from 0,1,2,3
        for (int g = 0; g < 4; ++g)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.000001f;
            }
            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    private void MakeFrequencyBands()
    {
        
        /// Split the song into four hertz values to be displayed on each line of the rythm game
        
        // Counts what sample should be added to, from 0,1,2,3
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            // average amplitude of all samples combined
            float average = 0;

            // adds the hertz perameters for each count to stick within
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            // adds the hertz values to the count
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                // then moves onto the next count
                count++;
            }
            
            average /= count;
            
            _freqBand[i] = average * 10;

            // Collect hertz values of each line, for use in DetectPeeks Script
            if (i == 0)
            {
                _greenLaneHertz = average;
                    
                //Debug.Log("Lowest hertz average = " + _currentHertz0);
            }
            if (i == 1)
            {
                _blueLaneHertz = average;
                //Debug.Log("Low-Mid hertz average = " + _currentHertz1);
            }
            if (i == 2)
            {
                _redLaneHertz = average;
                //Debug.Log("High-Mid hertz average = " + _currentHertz2);
            }
            if (i == 3)
            {
                _pinkLaneHertz = average;
                //Debug.Log("Highest hertz average = " + _currentHertz3);
            }

        }
    }

    /// Green Notes
    public void GreenAudioPeaks()
    {
        // set the current hertz value
        // compare the current hertz value to the previous one
        // update the previous hertz value and the current hertz value

        greenCurSpectrum = _greenLaneHertz;

        if (greenCurSpectrum - greenPrevSpectrum >= minDifference && canSpawnGreenNote == true)
        {
            if (detectPeaks.randomiseLanes == true && canSpawnRandomNote == true)
            {
                RandomBeat.Invoke();
                StartCoroutine(WaitForRandomNote());
            }
            else if (detectPeaks.randomiseLanes == false)
            {
                //Debug.Log("Blue Beat!");
                GreenBeat.Invoke();
                StartCoroutine(WaitForGreenNote());
            }
        }

        greenPrevSpectrum = greenCurSpectrum;

        /*for (int i = 0; i < 15; i++)
        {
            NoteLength[i] = _greenLaneHertz;
        }
        if (NoteLength.Count == 15)
        {
            NoteLength.RemoveAt(0);
        }
        float average = NoteLength.Average();
        float tolerance = 0.1f;
        if (average >= longNoteSensitivity)
        {
            //   Debug.Log("LongNoteSpawning");
        }
        // Check if all values are within the tolerance range
        bool allSimilar = NoteLength.All(val => Mathf.Abs(val - average) <= tolerance);

        if (allSimilar)
        {
            Debug.Log("All values are similar.");
        }
        else
        {
            Debug.Log("Values have too much variation.");
        }

        if (greenCurSpectrum - greenPrevSpectrum >= minDifference && canSpawnGreenNote == true)
        {

        }*/

    }

    public IEnumerator WaitForGreenNote()
    {
        if (canSpawnGreenNote == true)
        {
            canSpawnGreenNote = false;
            yield return new WaitForSeconds(timeBetweenBeats);
            canSpawnGreenNote = true;
        }
    }

    /// Blue Notes
    public void BlueAudioPeaks()
    {
        // set the current hertz value
        // compare the current hertz value to the previous one
        // update the previous hertz value and the current hertz value

        blueCurSpectrum = _blueLaneHertz;

        if (blueCurSpectrum - bluePrevSpectrum >= minDifference && canSpawnBlueNote == true)
        {
            if (detectPeaks.randomiseLanes == true && canSpawnRandomNote == true)
            {
                RandomBeat.Invoke();
                StartCoroutine(WaitForRandomNote());
            }
            else if (detectPeaks.randomiseLanes == false)
            {
                //Debug.Log("Blue Beat!");
                BlueBeat.Invoke();
                StartCoroutine(WaitForBlueNote());
            }

        }

        bluePrevSpectrum = blueCurSpectrum;

    }

    public IEnumerator WaitForBlueNote()
    {
        if (canSpawnBlueNote == true)
        {
            canSpawnBlueNote = false;
            yield return new WaitForSeconds(timeBetweenBeats);
            canSpawnBlueNote = true;
        }
    }

    /// Red Notes
    public void RedAudioPeaks()
    {
        // set the current hertz value
        // compare the current hertz value to the previous one
        // update the previous hertz value and the current hertz value

        redCurSpectrum = _redLaneHertz;

        if (redCurSpectrum - redPrevSpectrum >= minDifference && canSpawnRedNote == true)
        {
            if (detectPeaks.randomiseLanes == true && canSpawnRandomNote == true)
            {
                RandomBeat.Invoke();
                StartCoroutine(WaitForRandomNote());
            }
            else if (detectPeaks.randomiseLanes == false)
            {
                //Debug.Log("Red Beat!");
                RedBeat.Invoke();
                StartCoroutine(WaitForRedNote());
            }

        }

        redPrevSpectrum = redCurSpectrum;

    }


    public IEnumerator WaitForRedNote()
    {
        if (canSpawnRedNote == true)
        {
            canSpawnRedNote = false;
            yield return new WaitForSeconds(timeBetweenBeats);
            canSpawnRedNote = true;
        }
    }

    /// Pink Notes
    public void PinkAudioPeaks()
    {
        // set the current hertz value
        // compare the current hertz value to the previous one
        // update the previous hertz value and the current hertz value

        pinkCurSpectrum = _pinkLaneHertz;

        if (pinkCurSpectrum - pinkPrevSpectrum >= minDifference && canSpawnPinkNote == true)
        {
            if (detectPeaks.randomiseLanes == true && canSpawnRandomNote == true)
            {
                RandomBeat.Invoke();
                StartCoroutine(WaitForRandomNote());
            }
            else if (detectPeaks.randomiseLanes == false)
            {
                //Debug.Log("Pink Beat!");
                PinkBeat.Invoke();
                StartCoroutine(WaitForPinkNote());
            }

        }

        pinkPrevSpectrum = pinkCurSpectrum;

    }

    public IEnumerator WaitForPinkNote()
    {
        if (canSpawnPinkNote == true)
        {
            canSpawnPinkNote = false;
            yield return new WaitForSeconds(timeBetweenBeats);
            canSpawnPinkNote = true;
        }
    }

    public IEnumerator WaitForRandomNote()
    {
        if (canSpawnRandomNote == true)
        {
            canSpawnRandomNote = false;
            yield return new WaitForSeconds(timeBetweenBeats);
            canSpawnRandomNote = true;
        }
    }

    [System.Serializable]
    public class GreenBeatEventHandler : UnityEngine.Events.UnityEvent { }

    [System.Serializable]
    public class BlueBeatEventHandler : UnityEngine.Events.UnityEvent { }

    [System.Serializable]
    public class RedBeatEventHandler : UnityEngine.Events.UnityEvent { }

    [System.Serializable] 
    public class PinkBeatEventHandler : UnityEngine.Events.UnityEvent { }

    [System.Serializable]
    public class RandomLaneEventHandler : UnityEngine.Events.UnityEvent { }

}
