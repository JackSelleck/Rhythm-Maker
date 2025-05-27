using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ClickRedNotes : MonoBehaviour /// Detects Collisions for the note clicking and does the click anim
{
    public DetectPeaks DetectPeaks;
    public SetAudioDelay setAudioDelay;

    public GameObject button;
    public GameObject pressedButton;

    // Displays when a hit is done and its ranking
    public GameObject perfectPrefab;
    public Transform perfectSpawnPoint;
    public GameObject goodPrefab;
    public Transform goodSpawnPoint;
    public GameObject okayPrefab;
    public Transform okaySpawnPoint;
    public GameObject missPrefab;
    public Transform missSpawnPoint;
    public GameObject canvas;

    public bool notePressed = false;
    private bool longNotePressed = false;

    [SerializeField] public List<GameObject> Notes = new List<GameObject>();
    public int Points;
    public float Distance;
    public int Combo;
    public RaycastHit LongestCollidingNote;

    public int PerfectHit;
    public int GoodHit;
    public int OkayHit;
    public int MissedHit;
    public bool Miss = false;

    public float A_lastTimePressed;
    public float S_lastTimePressed;
    public float D_lastTimePressed;
    public float F_lastTimePressed;


    private void Update()
    {
        // button press animation
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.J)))
        {
            button.SetActive(false);
            pressedButton.SetActive(true);
        }
        else
        {
            button.SetActive(true);
            pressedButton.SetActive(false);
        }

        if (!Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.J)))
        {
            longNotePressed = false;
        }

        //CheckForNotes();
        //ClickNotesOnTime();
        HitGreenNotes();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Note"))
        {
            Notes.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Note"))
        { 
            Notes.Remove(other.gameObject);
        }
    }
    private void HitGreenNotes()
    {
        if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.J) && Notes.Count > 0))
        {
            if (setAudioDelay.rhythmGameStarted == true)
            {
                Distance = Vector3.Distance(Notes[0].gameObject.transform.position, this.transform.position);
                if (Distance <= 0.6f)
                {
                    Destroy(Notes[0].gameObject);
                    Notes.RemoveAt(0);
                    Points += 50;
                    Instantiate(perfectPrefab, perfectSpawnPoint.position, canvas.transform.rotation, canvas.transform);
                    //Debug.Log("Perfect, distance = " + Distance);
                    PerfectHit++;
                    Combo += 1;
                }
                if (Distance > 0.6f && Distance <= 1.8f)
                {
                    Destroy(Notes[0].gameObject);
                    Notes.RemoveAt(0);
                    Points += 30;
                    Instantiate(goodPrefab, goodSpawnPoint.position, canvas.transform.rotation, canvas.transform);
                    //Debug.Log("Good");
                    GoodHit++;
                    Combo += 1;
                }
                if (Distance > 1.8f && Distance <= 2.8f)
                {
                    Destroy(Notes[0].gameObject);
                    Notes.RemoveAt(0);
                    Points += 15;
                    Instantiate(okayPrefab, okaySpawnPoint.position, canvas.transform.rotation, canvas.transform);
                    //Debug.Log("Okay");
                    OkayHit++;
                    Combo += 1;
                }
                if (Distance > 2.8f)
                {
                    Points -= 50;
                    Instantiate(missPrefab, missSpawnPoint.position, canvas.transform.rotation, canvas.transform);
                    MissedHit++;
                }

            }
            else if (setAudioDelay.rhythmGameStarted == false)
            {
                Distance = Vector3.Distance(Notes[0].gameObject.transform.position, this.transform.position);
                if (Distance <= 1f)
                {
                    Destroy(Notes[0].gameObject);
                    Notes.RemoveAt(0);
                }
                if (Distance > 1f && Distance <= 2f)
                {
                    Destroy(Notes[0].gameObject);
                    Notes.RemoveAt(0);
                }
                if (Distance > 2f && Distance <= 2f)
                {
                    Destroy(Notes[0].gameObject);
                    Notes.RemoveAt(0);
                }
                if (Distance > 3f)
                {

                }
            }

        }
    }

    public void CheckForNotes()
    {

        // Create a ray from this object's position forward
       /* RaycastHit ForwardHit;
        RaycastHit BackHit;

        bool forwardCast = Physics.Raycast(transform.position, transform.forward, out ForwardHit, Mathf.Infinity);
        bool backCast = Physics.Raycast(transform.position, transform.forward * -1, out BackHit, Mathf.Infinity);
        LongestCollidingNote = ForwardHit;
        // Perform raycast
        if (backCast)
        {
            LongestCollidingNote = BackHit;
        }
        else
        {
            LongestCollidingNote = ForwardHit;
        }
        if (backCast || forwardCast)
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            Debug.DrawRay(transform.position, transform.forward * -1, Color.blue);
            // Check if the hit object has the target tag
            if (BackHit.transform.CompareTag("Note"))
            {
                Debug.Log("Hit object with tag: " + "targetTag");
                Debug.Log("Collided with backward ray");
                Distance = Vector3.Distance(transform.position, BackHit.point);

                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (Distance <= 1f)
                    {
                        BackHit.transform.gameObject.SetActive(false);
                        Points += 50f;
                        Debug.Log("Perfect, distance = " + Distance);
                        PerfectHit++;
                        // Combo += 0.1f;
                    }
                    if (Distance > 1f && Distance <= 3f)
                    {
                        BackHit.transform.gameObject.SetActive(false);
                        Points += 30f;
                        Debug.Log("Good");
                        GoodHit++;
                        // Combo += 0.1f;
                    }
                    if (Distance > 3f && Distance <= 4f)
                    {
                        BackHit.transform.gameObject.SetActive(false);
                        Points += 15f;;
                        Debug.Log("Okay");
                        OkayHit++;
                    }
                    if (Distance > 4f)
                    {
                        Points -= 50f;
                        MissedHit++;
                    }
                }
            }
            else if (ForwardHit.transform.CompareTag("Note"))
            {
                Debug.Log("Hit object with tag: " + "targetTag");
                Debug.Log("Collided with forward ray");
                Distance = Vector3.Distance(transform.position, ForwardHit.point);

                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (Distance <= 1f)
                    {
                        ForwardHit.transform.gameObject.SetActive(false);
                        Points += 50f;
                        Debug.Log("Perfect, distance = " + Distance);
                        PerfectHit++;
                        // Combo += 0.1f;
                    }
                    if (Distance > 1f && Distance <= 3f)
                    {
                        ForwardHit.transform.gameObject.SetActive(false);
                        Points += 30f;
                        Debug.Log("Good");
                        GoodHit++;
                        // Combo += 0.1f;
                    }
                    if (Distance > 3f && Distance <= 4f)
                    {
                        ForwardHit.transform.gameObject.SetActive(false);
                        Points += 15f; ;
                        Debug.Log("Okay");
                        OkayHit++;
                    }
                    if (Distance > 4f)
                    {
                        Points -= 50f;
                        MissedHit++;
                    }
                }
            }

            /*if (LongestCollidingNote.transform.CompareTag("LongNote"))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Points += 50f;
                    Debug.Log("Perfect");
                    ForwardHit.transform.gameObject.SetActive(false);
                    PerfectHit++;
                    longNotePressed = true;
                    if (Distance <= 1f)
                    {
                        Points += 50f;
                        Debug.Log("Perfect");
                        ForwardHit.transform.gameObject.SetActive(false);
                        PerfectHit++;
                        longNotePressed = true;
                        // Combo += 0.1f;
                    }
                    if (Distance > 1f && Distance <= 3f)
                    {
                        Points += 30f * Combo;
                        Debug.Log("Good");
                        ForwardHit.transform.gameObject.SetActive(false);
                        GoodHit++;
                        longNotePressed = true;
                        // Combo += 0.1f;
                    }
                    if (Distance > 3f && Distance <= 4f)
                    {
                        Points += 15f;
                        Debug.Log("Okay");
                        ForwardHit.transform.gameObject.SetActive(false);
                        OkayHit++;
                        longNotePressed = true;
                    }
                    if (Distance > 4f)
                    {
                        Points -= 50f;
                        MissedHit++;
                    }
                }

                //Debug.Log("LongGreenCollided");
            }

            /// If the long note is pressed and held down, the longNotePressed bool is set to true and allows the long line to be destroyed
            /// Once no longer held down, it is false, this makes it so the line can only be destroyed if pressed its note initially
            if (ForwardHit.transform.CompareTag("LongLine") && longNotePressed == true)
            {
                ForwardHit.transform.gameObject.SetActive(false);
                Points++;
            }
        } */
    }
    public void ClickNotesOnTime()
    {/*
        //loop through your list of times, get the difference between the current time and each iteration (element at the current index), then compare (that difference) to a threshold
        for (int i = 0; i < DetectPeaks.GreenNoteTimes.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.A))
            { 
                A_lastTimePressed = (float)AudioSettings.dspTime;
                if (DetectPeaks.GreenNoteTimes[i] >= A_lastTimePressed - 20 || DetectPeaks.GreenNoteTimes[i] <= A_lastTimePressed + 20)
                {
                    Debug.Log("Note Hit!");
                }
                else { Debug.Log("Distance between note and pressed = " + DetectPeaks.GreenNoteTimes[i] + " - " + A_lastTimePressed); }
            }

        }
        ///////////Debug.Log("GreenNoteTimesCount = " + DetectPeaks.GreenNoteTimes.Count); */
    }

}




