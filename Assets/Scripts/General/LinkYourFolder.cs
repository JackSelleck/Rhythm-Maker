using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkYourFolder : MonoBehaviour
{
    public GameObject linkFolderTutorial;
    public bool isLinked;

    private void Start()
    {
        linkFolderTutorial.SetActive(false);
    }

    public void ClickedFolderButton()
    {
        if (isLinked == false)
        {
            linkFolderTutorial.SetActive(true);
        }
    }

    public void ExitLinkingTutorial()
    {
        linkFolderTutorial.SetActive(false);
    }
}
