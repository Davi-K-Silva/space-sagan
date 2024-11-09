using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class OrbitDataLoader : MonoBehaviour
{
    public string planetID;  // Set this in the Inspector, e.g., "199" for Mercury
    public List<Vector3> positions = new List<Vector3>();

    void Start()
    {
        LoadOrbitData();
        DrawOrbit();
    }

    void LoadOrbitData()
    {
        // Load the text file from Resources
        TextAsset textAsset = Resources.Load<TextAsset>($"PlanetData/planet_{planetID}_positions");
        if (textAsset == null)
        {
            Debug.LogError($"Data file not found for planet ID {planetID}");
            return;
        }

        string[] lines = textAsset.text.Split('\n');
        foreach (string line in lines)
        {
            // Skip header and empty lines
            if (line.StartsWith("Date") || string.IsNullOrWhiteSpace(line)) continue;

            // Parse each line (Date, X, Y, Z)
            string[] parts = line.Trim().Split(',');
            if (parts.Length < 4) continue;

            float x = float.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
            float y = float.Parse(parts[2].Trim(), CultureInfo.InvariantCulture);
            float z = float.Parse(parts[3].Trim(), CultureInfo.InvariantCulture);

            // Add each position to the list
            positions.Add(new Vector3(x, y, z));
        }
    }

    void DrawOrbit()
{
    // Add a Line Renderer component if not already present
    LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

    // Set line renderer properties
    lineRenderer.positionCount = positions.Count;
    lineRenderer.SetPositions(positions.ToArray());

    // Set line width to make it more visible
    lineRenderer.startWidth = 2.0f; // Make it thicker (adjust as needed)
    lineRenderer.endWidth = 2.0f;

    // Set the color to white
    lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Assign a simple shader
    lineRenderer.startColor = Color.white;
    lineRenderer.endColor = Color.white;

    // Optional: Set a solid color without gradient
    lineRenderer.colorGradient = new Gradient
    {
        colorKeys = new[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
        alphaKeys = new[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
    };
}

}
