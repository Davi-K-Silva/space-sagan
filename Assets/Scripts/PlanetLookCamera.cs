using UnityEngine;

public class PlanetLookCamera : MonoBehaviour
{
    public Transform planetToFollow;
    public Transform targetToLookAt;

    [Header("Base Offsets (will be scaled)")]
    public float baseDistance = 5.0f;
    public float baseHeight = 2.0f;
    public float baseSide = 0.0f;

    void LateUpdate()
    {
        if (planetToFollow == null || targetToLookAt == null)
            return;

        // Compute scale based on the planet's size
        float scaleFactor = planetToFollow.localScale.magnitude;  // or use .x if uniform scaling

        // Calculate direction to target
        Vector3 lookDirection = (targetToLookAt.position - planetToFollow.position).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, lookDirection).normalized;

        // Scaled offsets
        float distance = baseDistance * scaleFactor;
        float height = baseHeight * scaleFactor;
        float side = baseSide * scaleFactor;

        // Final camera position
        Vector3 cameraPos = planetToFollow.position
                          - lookDirection * distance
                          + Vector3.up * height
                          + right * side;

        transform.position = cameraPos;
        transform.LookAt(targetToLookAt.position);
    }
}


