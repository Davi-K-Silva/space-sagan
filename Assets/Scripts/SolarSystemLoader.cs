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

    public float maxAnimationTime = 10.0f; // Maximum time for all planets to reach their final destination

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

    private Dictionary<string, Dictionary<string, Vector3>> positionData = new Dictionary<string, Dictionary<string, Vector3>>();
    private Dictionary<int, List<Vector3>> animationPaths = new Dictionary<int, List<Vector3>>();
    private Dictionary<int, int> planetFrames = new Dictionary<int, int>();
    private Dictionary<int, bool> planetAnimating = new Dictionary<int, bool>();
    private Dictionary<int, float> planetSpeeds = new Dictionary<int, float>(); // Stores calculated speed per planet

    void Start()
    {
        LoadAllPlanetData();
        UpdatePlanetsToTargetDate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrepareAnimationPaths();
            CalculateSpeeds();
            StartAnimation();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlanetsToStartingPosition();
        }

        AnimatePlanets();
    }

    void LoadAllPlanetData()
    {
        foreach (var objId in objectMapping.Keys)
        {
            LoadPlanetData(objId);
        }
    }

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

    void PrepareAnimationPaths()
    {
        animationPaths.Clear();
        planetFrames.Clear();
        planetAnimating.Clear();

        foreach (var obj in objectMapping)
        {
            int index = obj.Value;
            if (positionData.ContainsKey(obj.Key))
            {
                List<Vector3> path = new List<Vector3>(positionData[obj.Key].Values);
                animationPaths[index] = path;
                planetFrames[index] = 0;
                planetAnimating[index] = true;
            }
        }
    }

    // Calculate speed for each planet based on the max animation time
    void CalculateSpeeds()
    {
        foreach (var planet in animationPaths)
        {
            int planetIndex = planet.Key;
            List<Vector3> path = planet.Value;

            float pathLength = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                pathLength += Vector3.Distance(path[i], path[i + 1]);
            }

            // Calculate speed based on the total path length and max animation time
            planetSpeeds[planetIndex] = pathLength / maxAnimationTime;
        }
    }

    void StartAnimation()
    {
        List<int> planetKeys = new List<int>(planetAnimating.Keys);

        foreach (int planet in planetKeys)
        {
            planetAnimating[planet] = true;
        }
    }

    void AnimatePlanets()
    {
        foreach (var path in animationPaths)
        {
            int planetIndex = path.Key;
            List<Vector3> positions = path.Value;
            int frame = planetFrames[planetIndex];

            if (planetAnimating[planetIndex] && frame < positions.Count - 1)
            {
                Vector3 startPosition = positions[frame];
                Vector3 targetPosition = positions[frame + 1];

                // Use calculated speed for each planet based on maxAnimationTime
                planets[planetIndex].transform.position = Vector3.MoveTowards(
                    planets[planetIndex].transform.position,
                    targetPosition,
                    planetSpeeds[planetIndex] * Time.deltaTime
                );

                const float positionThreshold = 0.01f;

                if (Vector3.Distance(planets[planetIndex].transform.position, targetPosition) < positionThreshold)
                {
                    planetFrames[planetIndex]++;

                    if (planetFrames[planetIndex] >= positions.Count - 1)
                    {
                        planetAnimating[planetIndex] = false;
                    }
                }
            }
        }
    }

    void ResetPlanetsToStartingPosition()
    {
        foreach (var obj in objectMapping)
        {
            int index = obj.Value;
            planetFrames[index] = 0;
            planetAnimating[index] = false;

            if (positionData.ContainsKey(obj.Key))
            {
                List<Vector3> path = new List<Vector3>(positionData[obj.Key].Values);
                if (path.Count > 0)
                {
                    planets[index].transform.position = path[0];
                }
            }
        }
    }
}
