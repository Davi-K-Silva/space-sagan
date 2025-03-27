using System.Collections.Generic;
using UnityEngine;
using TMPro;  // For TMP_InputField

public class PlanetTeleporter : MonoBehaviour
{
    [SerializeField]
    private Transform[] planets;         // Assign planet transforms in the Inspector

    public Transform playerCamera;       // Assign the camera (or any object to teleport)
    public float teleportOffset = 10f;   // Offset distance from the planet


    void Update()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)(49 + i)))  // 49 is KeyCode for "1"
            {
                TeleportToPlanet(i);
            }
        }
    }

    // Teleport the player camera to the specified planet
    private void TeleportToPlanet(int index)
    {
        if (index < planets.Length)
        {
            Vector3 offset = planets[index].forward * teleportOffset;
            playerCamera.position = planets[index].position + offset;
            playerCamera.LookAt(planets[index]);  // Rotate camera to face the planet
            Debug.Log($"Teleported to {planets[index].name}");
        }
        else
        {
            Debug.LogError("Invalid planet index.");
        }
    }
}
