using UnityEngine;

public class LineToLabel : MonoBehaviour
{
    public Transform labelCanvas;  // Assign the Canvas in the Inspector
    private LineRenderer lineRenderer;

    void Start()
    {
        // Add a Line Renderer component if it does not exist
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Check if the lineRenderer was created successfully
        if (lineRenderer != null)
        {
            // Set line width for better visibility
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 10.0f;

            // Set the color to red
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Simple shader
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
        else
        {
            Debug.LogError("Failed to create LineRenderer component.");
        }
    }

    void Update()
    {
        if (lineRenderer != null && labelCanvas != null)
        {
            lineRenderer.SetPosition(0, transform.position);       // Start at planet position
            lineRenderer.SetPosition(1, labelCanvas.position);     // End at label position
        }
        else
        {
            if (lineRenderer == null)
                Debug.LogError("LineRenderer component is missing.");
            if (labelCanvas == null)
                Debug.LogError("Label canvas reference is missing.");
        }
    }
}
