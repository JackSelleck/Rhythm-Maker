using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene("Play Song");
    }
}
