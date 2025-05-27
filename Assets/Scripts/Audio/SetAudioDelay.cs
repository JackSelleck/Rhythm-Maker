using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetAudioDelay : MonoBehaviour
{
    public DetectPeaks DetectPeaks;

    public ClickGreenNotes clickGreenNotes;
    public ClickBlueNotes clickBlueNotes;
    public ClickRedNotes clickRedNotes;
    public ClickPinkNotes clickPinkNotes;

    public Canvas LoadingPerfectCallibration;

    //The offset to the first beat of the song in seconds
    public float previewFirstBeatOffset;
    public float firstBeatOffset;

    // For the preview
    public float timeFirstPreviewNoteCollides = 0;
    private bool firstPreviewNoteCollided = false;
    public float timeBetweenPreviewInstantiationAndReachingDestination;

    // For the actual game
    public float timeFirstNoteCollides = 0;
    private bool firstNoteCollided = false;
    public float timeBetweenInstantiationAndReachingDestination;

    public bool previewSongStarted = false;
    public bool songStarted;
    /// to be used by other scripts
    public bool rhythmGameStarted = false;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource analysisMusic;
    public AudioSource playedMusic;

    public GameObject RestartButton;
    public bool StartDestroyingCallibrationNotes = false;
    public bool StartDestroyingNotes = false;
    public bool NotesAreDestroyed = false;

    void Start()
    {
        RestartButton.SetActive(false);
        LoadingPerfectCallibration.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note") && firstPreviewNoteCollided == false)
        {
            timeFirstPreviewNoteCollides = (float)AudioSettings.dspTime;
            firstPreviewNoteCollided = true;
        }

        if (other.CompareTag("Note") && firstNoteCollided == false && NotesAreDestroyed == true)
        {          
            timeFirstNoteCollides = (float)AudioSettings.dspTime;
            firstNoteCollided = true;
            StartDestroyingCallibrationNotes = true;
            //Debug.Log("NoteCollidedTimeFound");
        }
    }

    void Update()
    {
        // Calculate the appropriate delay for the song
        while (timeFirstPreviewNoteCollides <= 0)
        {
            timeBetweenPreviewInstantiationAndReachingDestination = DetectPeaks.timeFirstNoteInstantiated - timeFirstPreviewNoteCollides;
            previewFirstBeatOffset = (float)AudioSettings.dspTime - timeBetweenPreviewInstantiationAndReachingDestination;
            return; 
        }

        if (timeFirstPreviewNoteCollides > 0 && previewSongStarted == false)         
        {
            previewSongStarted = true;
            //playedMusic.PlayDelayed(previewFirstBeatOffset);
            //Debug.Log("Preview Song Started!");
            RestartButton.SetActive(true);
        }

        DestroyNotes();

        while (timeFirstNoteCollides <= 0)
        {
            timeBetweenInstantiationAndReachingDestination = DetectPeaks.timeFirstNoteInstantiated - timeFirstNoteCollides;
            firstBeatOffset = (float)AudioSettings.dspTime - timeBetweenInstantiationAndReachingDestination;
            return;
        }

        if (timeFirstNoteCollides > 0 && songStarted == false && StartDestroyingNotes == false)
        {
            songStarted = true;
            rhythmGameStarted = true;
            analysisMusic.Play();
            playedMusic.PlayDelayed(firstBeatOffset);
            //Debug.Log("Song Started!");
            LoadingPerfectCallibration.enabled = false;
        }
    }

    public void RecallibrateSong()
    {
        playedMusic.PlayDelayed(previewFirstBeatOffset);
        analysisMusic.Play();
    }

    public void SongConfirmed()
    {
        StartDestroyingNotes = true;
    }

    public void DestroyNotes()
    {
        if (StartDestroyingNotes == true)
        {
            /// This occurs after the song settings are confirmed
            GameObject[] Note = GameObject.FindGameObjectsWithTag("Note");
            while (Note.Length > 0)
            {
                Note[0].SetActive(false);
                NotesAreDestroyed = false;
                return;
            }

            // Stops a bug occuring where notes cannot be clicked if a note was destroyed within the clicking radius
            if (Note.Length <= 0)
            {
                if (clickGreenNotes.Notes.Count > 0)
                {
                    clickGreenNotes.Notes.Clear();
                }
                if (clickBlueNotes.Notes.Count > 0)
                {
                    clickBlueNotes.Notes.Clear();
                }
                if (clickRedNotes.Notes.Count > 0)
                {
                    clickRedNotes.Notes.Clear();
                }
                if (clickPinkNotes.Notes.Count > 0)
                {
                    clickPinkNotes.Notes.Clear();
                }
                NotesAreDestroyed = true;
                StartDestroyingNotes = false;
                Callibration();
                //Debug.Log("NotesDestroyed");
            }

        }
        if (StartDestroyingCallibrationNotes == true)
        {
            /// This occurs after the song is callibrated
            GameObject[] Note = GameObject.FindGameObjectsWithTag("Note");
            while (Note.Length > 0)
            {
                Note[0].SetActive(false);
                return;
            }

            // Stops a bug occuring where notes cannot be clicked if a note was destroyed within the clicking radius
            if (Note.Length <= 0)
            {
                if (clickGreenNotes.Notes.Count > 0)
                {
                    clickGreenNotes.Notes.Clear();
                }
                if (clickBlueNotes.Notes.Count > 0)
                {
                    clickBlueNotes.Notes.Clear();
                }
                if (clickRedNotes.Notes.Count > 0)
                {
                    clickRedNotes.Notes.Clear();
                }
                if (clickPinkNotes.Notes.Count > 0)
                {
                    clickPinkNotes.Notes.Clear();
                }
                StartDestroyingCallibrationNotes = false;
                //Debug.Log("NotesDestroyed");
            }

        }
    }

    private void Callibration()
    {
        // Resets values which were used by the song preview
        // So the rhythm game can be perfectly in time when it starts
        if (NotesAreDestroyed == true)
        {
            LoadingPerfectCallibration.enabled = true;
            analysisMusic.Play();
            playedMusic.Stop();
            DetectPeaks.firstNoteGenerated = false;
            DetectPeaks.timeFirstNoteInstantiated = 0;
            timeFirstNoteCollides = 0;
        }

    }

}
// TIME BETWEEN WHEN THE SONG STARTS AND WHEN THE FIRST GENERATED NOTE WILL REACH IT
//songstart.time - noteReached.time = RythmSongStartTime

//Time between first note generated and how long it takes to gert there

//PLAY THE AUDIO GET THE TIME BETWEEN WHEN THE SONG STARTED, GET THE TIME THE FIRST NOTE PLAYS AND MINUS IT FROM WHEN THE NOTE REACHED THERE