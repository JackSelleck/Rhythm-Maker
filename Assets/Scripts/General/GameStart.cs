using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;  // For LINQ methods (optional)
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class GameStart : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject ChooseAudio;
    public Button gameStart;

    private void Awake()
    {
        if (gameStart != null)
        {
            gameStart.onClick.AddListener(GoToChooseAudio);
        }
    }

    private void Start()
    {
        ChooseAudio.SetActive(false);
    }

    private void GoToChooseAudio()
    {
        ChooseAudio.SetActive(true);
        StartMenu.SetActive(false);
    }

}
