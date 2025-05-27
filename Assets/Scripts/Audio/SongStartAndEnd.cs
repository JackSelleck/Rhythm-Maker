using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongStartAndEnd : MonoBehaviour
{
    public AudioSource source;
    public SetAudioDelay setAudioDelay;
    public Canvas RhythmSettings;
    public Canvas ScoreSettings;
    public Canvas EndOfGameRanking;
    public bool SongOver = false;

    // Start is called before the first frame update
    void Start()
    {
        ScoreSettings.enabled = false;
        EndOfGameRanking.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (source.isPlaying && setAudioDelay.rhythmGameStarted == true)
        {
            RhythmSettings.enabled = false;
            ScoreSettings.enabled = true;
        }
        if (!source.isPlaying && setAudioDelay.rhythmGameStarted == true)
        {
            GameObject[] Note = GameObject.FindGameObjectsWithTag("Note");
            if (Note.Length <= 0)
            {
                ScoreSettings.enabled = false;
                EndOfGameRanking.enabled = true;
            }
        }
    }
}
