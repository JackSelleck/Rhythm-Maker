using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPeaks : MonoBehaviour
{
    public SetAudioDelay setAudioDelay;

    public GameObject GreenNotePrefab;
    public Transform GreenNoteSpawnPoint;
    //public List<float> GreenNoteTimes = new List<float>();

    public float PotentialPoints;

    public GameObject BlueNotePrefab;
    public Transform BlueNoteSpawnPoint;
    //public List<float> BlueNoteTimes = new List<float>();

    public GameObject RedNotePrefab;
    public Transform RedNoteSpawnPoint;
    //public List<float> RedNoteTimes = new List<float>();

    public GameObject PinkNotePrefab;
    public Transform PinkNoteSpawnPoint;
    //public List<float> PinkNoteTimes = new List<float>();

    public bool firstNoteGenerated = false;
    public float timeFirstNoteInstantiated;

    public bool randomiseLanes;
    [SerializeField] private bool lane0Active;
    [SerializeField] private bool lane1Active;
    [SerializeField] private bool lane2Active;
    [SerializeField] private bool lane3Active;

    public void InstantiateGreenNote()
    {
        // The user will adjust the time between when the notes are made and when they should be pressed, based on the song timing
        // they will simply have a scroll wheel which will let them add a an extra amount of time to the added floats in the list
        // This SHOULD make it so the delayed audio track which is played to the player matches with the notes

        // could also change the time the overlayed track is played instead?
        //GreenNoteTimes.Add((float)AudioSettings.dspTime);
        /// if it collides for a certain amount of time, i can spawn in a long note and stop spawning the long lines when it exits?
        //Instantiate(GreenNotePrefab, GreenNoteSpawnPoint.position, GreenNoteSpawnPoint.transform.rotation);
        GameObject InstantiatedGreenNote = Instantiate(GreenNotePrefab, GreenNoteSpawnPoint.position, GreenNoteSpawnPoint.rotation);
        InstantiatedGreenNote.name = "GreenNote";

        if (firstNoteGenerated == false)
        {
            timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
            firstNoteGenerated = true;
        }
        if (setAudioDelay.rhythmGameStarted == true)
        {
            PotentialPoints += 50;
        }

    }
    public void InstantiateBlueNote()
    {
        //GreenNoteTimes.Add((float)AudioSettings.dspTime);
        GameObject InstantiatedBlueNote = Instantiate(BlueNotePrefab, BlueNoteSpawnPoint.position, BlueNoteSpawnPoint.rotation);
        InstantiatedBlueNote.name = "BlueNote";

        if (firstNoteGenerated == false)
        {
            timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
            firstNoteGenerated = true;
        }
        if (setAudioDelay.rhythmGameStarted == true)
        {
            PotentialPoints += 50;
        }
    }
    public void InstantiateRedNote()
    {
        //GreenNoteTimes.Add((float)AudioSettings.dspTime);
        GameObject InstantiatedRedNote = Instantiate(RedNotePrefab, RedNoteSpawnPoint.position, RedNoteSpawnPoint.rotation);
        InstantiatedRedNote.name = "RedNote";

        if (firstNoteGenerated == false)
        {
            timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
            firstNoteGenerated = true;
        }
        if (setAudioDelay.rhythmGameStarted == true)
        {
            PotentialPoints += 50;
        }
    }
    public void InstantiatePinkNote()
    {
        //GreenNoteTimes.Add((float)AudioSettings.dspTime);
        GameObject InstantiatedPinkNote = Instantiate(PinkNotePrefab, PinkNoteSpawnPoint.position, PinkNoteSpawnPoint.rotation);
        InstantiatedPinkNote.name = "PinkNote";

        if (firstNoteGenerated == false)
        {
            timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
            firstNoteGenerated = true;
        }
        if (setAudioDelay.rhythmGameStarted == true)
        {
            PotentialPoints += 50;
        }
    }
   /* public void RandomisedLanes()
    {
        PotentialPoints += 50;
        int LaneToSpawn = Random.Range(0, 4);
        int LaneToSpawn2 = Random.Range(0, 4);
        int LaneToSpawn3 = Random.Range(0, 4);
        int LaneToSpawn4 = Random.Range(0, 4);

        if (LaneToSpawn == 0 || LaneToSpawn2 == 0 || LaneToSpawn3 == 0 || LaneToSpawn4 == 0)
        {
            GameObject InstantiatedGreenNote = Instantiate(GreenNotePrefab, GreenNoteSpawnPoint.position, GreenNoteSpawnPoint.rotation, this.transform);
            InstantiatedGreenNote.name = "GreenNote";

            if (firstNoteGenerated == false)
            {
                timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
                firstNoteGenerated = true;
            }
        }
        if (LaneToSpawn == 1 || LaneToSpawn2 == 0 || LaneToSpawn3 == 0 || LaneToSpawn4 == 0)
        {
            GameObject InstantiatedBlueNote = Instantiate(BlueNotePrefab, BlueNoteSpawnPoint.position, BlueNoteSpawnPoint.rotation, this.transform);
            InstantiatedBlueNote.name = "BlueNote";

            if (firstNoteGenerated == false)
            {
                timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
                firstNoteGenerated = true;
            }
        }
        if (LaneToSpawn == 2 || LaneToSpawn2 == 0 || LaneToSpawn3 == 0 || LaneToSpawn4 == 0)
        {
            //GreenNoteTimes.Add((float)AudioSettings.dspTime);
            GameObject InstantiatedRedNote = Instantiate(RedNotePrefab, RedNoteSpawnPoint.position, RedNoteSpawnPoint.rotation, this.transform);
            InstantiatedRedNote.name = "RedNote";

            if (firstNoteGenerated == false)
            {
                timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
                firstNoteGenerated = true;
            }
        }
        if (LaneToSpawn == 3 || LaneToSpawn2 == 0 || LaneToSpawn3 == 0 || LaneToSpawn4 == 0)
        {
            GameObject InstantiatedPinkNote = Instantiate(PinkNotePrefab, PinkNoteSpawnPoint.position, PinkNoteSpawnPoint.rotation, this.transform);
            InstantiatedPinkNote.name = "PinkNote";

            if (firstNoteGenerated == false)
            {
                timeFirstNoteInstantiated = (float)AudioSettings.dspTime;
                firstNoteGenerated = true;
            }
        }
    }*/
}
