using UnityEngine;

public class DistanceBasedScaler : MonoBehaviour
{
    public Transform planet;             // Reference to the planet's transform
    public float labelScaleFactor = 0.1f; // Controls the label's base scale
    public float positionOffset = 0.5f;   // Controls label distance from the planet
    public float minScale = 0.5f;         // Minimum label scale
    public float maxScale = 5f;           // Maximum label scale

    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        // Calculate distance between the camera and the planet
        float distanceToCamera = Vector3.Distance(mainCamera.position, planet.position);

        // Set a uniform scale based on distance to the camera, independent of planet scale
        float scale = Mathf.Clamp(distanceToCamera * labelScaleFactor, minScale, maxScale);
        transform.localScale = Vector3.one * scale;

        // Position the label away from the planet based on distance to the camera
        Vector3 directionFromPlanet = (transform.position - planet.position).normalized;
        transform.position = planet.position + directionFromPlanet * distanceToCamera * positionOffset;
    }
}
