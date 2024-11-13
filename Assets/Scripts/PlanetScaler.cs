using System.Collections.Generic;
using UnityEngine;

public class PlanetScaler : MonoBehaviour
{
    public GameObject[] planets;         // Assign the planet GameObjects in the Inspector
    public float scaleStep = 0.01f;      // The increment for scaling (e.g., 0.01 means 1% increase per press)

    private Dictionary<GameObject, Vector3> baseScales = new Dictionary<GameObject, Vector3>();
    private float scaleFactor = 1.0f;    // Starting scale factor

    void Start()
    {
        // Store each planet's original scale
        foreach (var planet in planets)
        {
            baseScales[planet] = planet.transform.localScale;
        }
    }

    void Update()
    {
        // Increase scale factor when pressing "K"
        if (Input.GetKey(KeyCode.K))
        {
            scaleFactor += scaleStep;
            ApplyScale();
        }

        // Decrease scale factor when pressing "L"
        if (Input.GetKey(KeyCode.L))
        {
            scaleFactor = Mathf.Max(0.1f, scaleFactor - scaleStep); // Prevent scale factor from going below 0.1
            ApplyScale();
        }
    }

    void ApplyScale()
    {
        string scaleReport = "Current Planet Scales:\n";

        foreach (var planet in planets)
        {
            Vector3 newScale = baseScales[planet] * scaleFactor;
            planet.transform.localScale = newScale;
            scaleReport += $"{planet.name}: {newScale}\n";
        }

        Debug.Log(scaleReport); // Print consolidated report
    }
}
