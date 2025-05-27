using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profiler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ///Debug.Log(UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / 1024 + " KB used");
    }
}
