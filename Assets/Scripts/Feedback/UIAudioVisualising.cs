using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioVisualising : MonoBehaviour
{
    public AudioSource grabbedAudio;

    public int _band;
    public float _startScale, _scaleMultiplier;
    public bool _useBuffer;

    private float[] _samples = new float[2048];
    private float[] _freqBand = new float[8];
    private float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];

    private void Update()
    {
        BandBuffer();
        Visualise();
        MakeFrequencyBands();
        GetSpectrumAudioSource();
    }

    private void GetSpectrumAudioSource()
    {
        // Gets the hertz values of the current song
        grabbedAudio.GetSpectrumData(_samples, 0, FFTWindow.BlackmanHarris);
    }

    private void BandBuffer()
    {
        // Buffers the audio visualiser to have less erratic movement
        // Counts what sample should be buffered, from 0,1,2,3
        for (int g = 0; g < 8; ++g)
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

    private void Visualise()
    {
        if (_useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (_bandBuffer[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
        }
        if (!_useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (_freqBand[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
        }
    }

    private void MakeFrequencyBands()
    {

        /// Split the song into hertz values to display on the visualiser

        // Counts what sample should be added to, from 0,1,2,3
        int count = 0;
        for (int i = 0; i < 8; i++)
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

        }
    }

}
