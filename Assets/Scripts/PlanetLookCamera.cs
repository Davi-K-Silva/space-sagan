using UnityEngine;

public class PlanetLookCamera : MonoBehaviour
{
    public Transform planetToFollow;
    public Transform targetToLookAt;

    [Header("Base Offsets (will be scaled)")]
    public float baseDistance = 1.0f;
    public float baseHeight = 0.5f;
    public float baseSide = 0.5f;

    private float currentDistance;
    private float currentHeight;
    private float currentSide;

    void Start()
    {
        SetOffsets(baseDistance, baseHeight, baseSide); // Init with base
    }

    public void SetOffsets(float distance, float height, float side)
    {
        currentDistance = distance;
        currentHeight = height;
        currentSide = side;
    }

    public void SetTargets(Transform planet, Transform lookAt)
    {
        planetToFollow = planet;
        targetToLookAt = lookAt;
    }

    void LateUpdate()
    {
        if (planetToFollow == null || targetToLookAt == null)
            return;

        float scaleFactor = planetToFollow.localScale.magnitude;

        Vector3 lookDirection = (targetToLookAt.position - planetToFollow.position).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, lookDirection).normalized;

        float distance = currentDistance * scaleFactor;
        float height = currentHeight * scaleFactor;
        float side = currentSide * scaleFactor;

        Vector3 cameraPos = planetToFollow.position
                          - lookDirection * distance
                          + Vector3.up * height
                          + right * side;

        //transform.position = cameraPos;
        //transform.LookAt(targetToLookAt.position);

        // Smooth position and rotation
        float smoothSpeed = 5f;
        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * smoothSpeed);

        Quaternion desiredRotation = Quaternion.LookRotation(targetToLookAt.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * smoothSpeed);
    }
}



