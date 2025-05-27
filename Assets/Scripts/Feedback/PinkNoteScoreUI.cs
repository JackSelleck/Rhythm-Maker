using UnityEngine;
using TMPro;

public class PinkNoteScoreUI : MonoBehaviour
{
    private TextMeshProUGUI Transparency;
    private Rigidbody Rigidbody;
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Transparency = GetComponent<TextMeshProUGUI>();
    }
    private void FixedUpdate()
    {
        Rigidbody.velocity = new Vector3(26, -100, 0);

        while (Transparency.alpha > 0)
        {
            Transparency.alpha -= 0.015f;
            return;
        }
        if (Transparency.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
