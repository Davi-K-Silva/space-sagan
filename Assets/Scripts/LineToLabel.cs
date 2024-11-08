using UnityEngine;

public class LineToLabel : MonoBehaviour
{
    public Transform labelCanvas;  // Assign the Canvas in the Inspector

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);       // Start at planet position
        lineRenderer.SetPosition(1, labelCanvas.position);     // End at label position
    }
}
