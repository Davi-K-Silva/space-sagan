using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SolarSystemLoader : MonoBehaviour
{
    [Tooltip("Date to load positions for, in YYYY-MM-DD format.")]
    public string targetDate; // Set this in the Inspector

    [SerializeField]
    private GameObject[] planets;  // Assign planet GameObjects in the Inspector

    // Object ID to GameObject mapping
    private Dictionary<string, int> objectMapping = new Dictionary<string, int>
    {
        { "10", 0 },  // Sun
        { "199", 1 }, // Mercury
        { "299", 2 }, // Venus
        { "399", 3 }, // Earth
        { "499", 4 }, // Mars
        { "599", 5 }, // Jupiter
        { "699", 6 }, // Saturn
        { "799", 7 }, // Uranus
        { "899", 8 }  // Neptune
    };

    // Dictionary to store positions by date for each object
    private Dictionary<string, Dictionary<string, Vector3>> positionData = new Dictionary<string, Dictionary<string, Vector3>>();

    void Start()
    {
        LoadAllPlanetData();
        UpdatePlanetsToTargetDate();
    }

    // Load data for each planet from its respective file in Resources
    void LoadAllPlanetData()
    {
        foreach (var objId in objectMapping.Keys)
        {
            LoadPlanetData(objId);
        }
    }

    // Load data for a single planet based on its ID from Resources
    void LoadPlanetData(string objId)
    {
        // Load the text file as a TextAsset from the Resources folder
        TextAsset textAsset = Resources.Load<TextAsset>($"PlanetData/planet_{objId}_positions");
        if (textAsset == null)
        {
            Debug.LogError($"Data file not found for object {objId}");
            return;
        }

        Dictionary<string, Vector3> planetPositions = new Dictionary<string, Vector3>();

        // Read lines from the TextAsset's text content
        string[] lines = textAsset.text.Split('\n');
        foreach (string line in lines)
        {
            // Skip header or empty lines
            if (line.StartsWith("Date") || string.IsNullOrWhiteSpace(line)) continue;

            // Parse the line to extract date and position
            string[] parts = line.Split(',');
            if (parts.Length < 4) continue;

            string date = parts[0].Trim();
            float x = float.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
            float y = float.Parse(parts[2].Trim(), CultureInfo.InvariantCulture);
            float z = float.Parse(parts[3].Trim(), CultureInfo.InvariantCulture);

            planetPositions[date] = new Vector3(x, y, z);
        }

        // Store the loaded data in the main dictionary
        positionData[objId] = planetPositions;
    }

    // Update planet positions to match the target date
    void UpdatePlanetsToTargetDate()
    {
        foreach (var obj in objectMapping)
        {
            string objId = obj.Key;
            int index = obj.Value;

            if (positionData.ContainsKey(objId) && positionData[objId].TryGetValue(targetDate, out Vector3 position))
            {
                planets[index].transform.position = position;
            }
            else
            {
                Debug.LogWarning($"No position data found for object {objId} on {targetDate}");
            }
        }
    }
}
