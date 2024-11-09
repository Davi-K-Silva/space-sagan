using UnityEngine;

public class LabelDistanceFade : MonoBehaviour
{
    public Transform planet;        // Reference to the planet this label is attached to
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    public float hideDistance = 300f;  // Distance at which the line disappears

    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform; // Reference to the main camera
    }

    void Update()
    {
        // Calculate the distance between the camera and the planet
        float distance = Vector3.Distance(mainCamera.position, planet.position);

        // Enable or disable the line based on the distance
        if (distance < hideDistance)
        {
            lineRenderer.enabled = false;  // Hide the line when close
        }
        else
        {
            lineRenderer.enabled = true;   // Show the line when far away
        }
    }
}
