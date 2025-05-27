using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadSongsFromPath : MonoBehaviour
{
    /*public FileDragAndDrop FileDragAndDrop;
    public AudioSource LoadedAudioSource;
    public float songLength = 0;
    public bool playSong = false;

    [System.Obsolete]
    public void Update()
    {
        if (FileDragAndDrop.LoadFromPath==true)
        {
            StartCoroutine(LoadSongCorutine());
        }
        
    }

    [System.Obsolete]
    IEnumerator LoadSongCorutine()
    {
        string url = string.Format("file://{0}", FileDragAndDrop.songPath);
        WWW www = new WWW(url);
        yield return www;

        LoadedAudioSource.clip = NAudioPlayer.FromMp3Data(www.bytes);
        songLength = LoadedAudioSource.clip.length;
        playSong = true;

    }*/
}
