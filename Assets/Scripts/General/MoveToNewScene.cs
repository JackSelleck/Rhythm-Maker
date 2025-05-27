using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNewScene : MonoBehaviour
{
    public void LoadRhythmGame()
    {
        SceneManager.LoadScene("PlaySong");
    }
}
