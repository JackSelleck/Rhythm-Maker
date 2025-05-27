using UnityEngine;

public class LightButtons : MonoBehaviour
{
    public GameObject[] objects; // Assign 4 GameObjects for A, S, D, F
    public Color black;
    public Color white;

    private KeyCode[] keys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
    private Renderer[] renderers;
    private Color[] targetColors;
    private Color[] targetEmissions;
    private float maxGlowIntensity = 0.5f;
    private float transitionSpeed = 12f;

    void Start()
    {
        if (objects.Length != keys.Length)
        {
            Debug.LogError("Incorrect amount of buttons or keys", this);
            enabled = false;
            return;
        }

        renderers = new Renderer[objects.Length];
        targetColors = new Color[objects.Length];
        targetEmissions = new Color[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            renderers[i] = objects[i].GetComponent<Renderer>();
            renderers[i].material.EnableKeyword("_EMISSION"); // Enable emission
            targetColors[i] = black;
            targetEmissions[i] = Color.black; // No emission at start
        }
    }

    void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            bool isPressed = Input.GetKey(keys[i]);
            targetColors[i] = isPressed ? white : black;

            // If pressed, set emission color. If not, fade it out smoothly.
            if (isPressed)
            {
                targetEmissions[i] = white * maxGlowIntensity;
            }
            else
            {
                targetEmissions[i] = Color.Lerp(renderers[i].material.GetColor("_EmissionColor"), Color.black, transitionSpeed * Time.deltaTime);
            }

            // Smoothly transition base color
            renderers[i].material.color = Color.Lerp(renderers[i].material.color, targetColors[i], transitionSpeed * Time.deltaTime);

            // Smoothly transition emission
            renderers[i].material.SetColor("_EmissionColor", targetEmissions[i]);
        }
    }
}
