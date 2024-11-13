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
    public Dictionary<string, Dictionary<string, Vector3>> positionData = new Dictionary<string, Dictionary<string, Vector3>>();
    private Dictionary<int, List<Vector3>> animationPaths = new Dictionary<int, List<Vector3>>();
    private Dictionary<int, bool> planetAnimating = new Dictionary<int, bool>();
    
    private float elapsedTime = 0f; // Track the elapsed animation time
    private bool isAnimating = false;

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
            StartAnimation();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlanetsToStartingPosition();
        }

        if (isAnimating)
        {
            AnimatePlanets();
        }
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

    public void UpdatePlanetsToTargetDate()
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
        planetAnimating.Clear();

        foreach (var obj in objectMapping)
        {
            int index = obj.Value;
            if (positionData.ContainsKey(obj.Key))
            {
                List<Vector3> path = new List<Vector3>(positionData[obj.Key].Values);
                animationPaths[index] = path;
                planetAnimating[index] = true;
            }
        }
    }

    void StartAnimation()
    {
        elapsedTime = 0f;
        isAnimating = true;
    }

    void AnimatePlanets()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / maxAnimationTime); // Calculate interpolation factor based on max time

        foreach (var path in animationPaths)
        {
            int planetIndex = path.Key;
            List<Vector3> positions = path.Value;

            if (positions.Count > 1 && planetAnimating[planetIndex])
            {
                // Calculate the segment based on the interpolation factor
                int totalSegments = positions.Count - 1;
                float segmentProgress = t * totalSegments;
                int currentSegment = Mathf.FloorToInt(segmentProgress);
                float segmentT = segmentProgress - currentSegment; // Interpolation within the segment

                if (currentSegment < totalSegments)
                {
                    Vector3 startPosition = positions[currentSegment];
                    Vector3 targetPosition = positions[currentSegment + 1];
                    planets[planetIndex].transform.position = Vector3.Lerp(startPosition, targetPosition, segmentT);
                }
                else
                {
                    planets[planetIndex].transform.position = positions[totalSegments]; // Ensure final position
                    planetAnimating[planetIndex] = false;
                }
            }
        }

        // Stop the animation when all planets have reached their final positions
        if (t >= 1f)
        {
            isAnimating = false;
        }
    }

    void ResetPlanetsToStartingPosition()
    {
        isAnimating = false;
        elapsedTime = 0f;

        foreach (var obj in objectMapping)
        {
            int index = obj.Value;
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
