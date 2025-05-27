using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Note"))
        {
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("LongNote"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("LongLine"))
        {
            other.gameObject.SetActive(false);
        }

    }

}
