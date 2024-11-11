using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SolarSystemLoader : MonoBehaviour
{
    [Tooltip("Date to load positions for, in YYYY-MM-DD format.")]
    public string targetDate; // Set this in the Inspector

    [SerializeField]
    private GameObject[] planets;  // Assign planet GameObjects in the Inspector

    public float animationSpeed = 1.0f; // Speed multiplier for the animation

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
    private bool isAnimating = false;

    void Start()
    {
        LoadAllPlanetData();
        UpdatePlanetsToTargetDate();
    }

    void Update()
    {
        // Start or stop animation when the "P" key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            isAnimating = !isAnimating;

            if (isAnimating)
            {
                StartCoroutine(AnimatePlanets());
            }
            else
            {
                StopCoroutine(AnimatePlanets());
            }
        }
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
        TextAsset textAsset = Resources.Load<TextAsset>($"PlanetData/planet_{objId}_positions");
        if (textAsset == null)
        {
            Debug.LogError($"Data file not found for object {objId}");
            return;
        }

        Dictionary<string, Vector3> planetPositions = new Dictionary<string, Vector3>();

        string[] lines = textAsset.text.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("Date") || string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');
            if (parts.Length < 4) continue;

            string date = parts[0].Trim();
            float x = float.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
            float y = float.Parse(parts[2].Trim(), CultureInfo.InvariantCulture);
            float z = float.Parse(parts[3].Trim(), CultureInfo.InvariantCulture);

            planetPositions[date] = new Vector3(x, y, z);
        }

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

    // Coroutine to animate planets along their paths
    IEnumerator AnimatePlanets()
    {
        // Dictionary to keep track of each planet's position index
        Dictionary<string, int> currentIndices = new Dictionary<string, int>();

        // Initialize the indices for each planet to start at the first position
        foreach (var objId in objectMapping.Keys)
        {
            currentIndices[objId] = 0;
        }

        while (isAnimating)
        {
            foreach (var obj in objectMapping)
            {
                string objId = obj.Key;
                int index = obj.Value;

                if (positionData.ContainsKey(objId))
                {
                    var positions = positionData[objId];
                    var dates = new List<string>(positions.Keys);
                    dates.Sort(); // Sort dates to follow the timeline

                    if (currentIndices[objId] < dates.Count)
                    {
                        string date = dates[currentIndices[objId]];
                        Vector3 targetPosition = positions[date];
                        planets[index].transform.position = Vector3.Lerp(
                            planets[index].transform.position,
                            targetPosition,
                            Time.deltaTime * animationSpeed
                        );

                        // Move to the next position if close enough
                        if (Vector3.Distance(planets[index].transform.position, targetPosition) < 0.01f)
                        {
                            currentIndices[objId] = (currentIndices[objId] + 1) % dates.Count;
                        }
                    }
                }
            }

            yield return null;
        }
    }
}
