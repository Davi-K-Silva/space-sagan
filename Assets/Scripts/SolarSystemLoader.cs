using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SolarSystemLoader : MonoBehaviour
{
    [Tooltip("Date to load positions for, in YYYY-MM-DD format.")]
    public string targetDate;             // Set this in the Inspector
    public string startDate;              // Animation StartDate 
    public string endDate;                // Animation EndDate
    public string dataDir = "default";    // Planeta data dir 

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
    private Dictionary<int, float> axialTiltDegrees = new Dictionary<int, float>
    {
        { 0, 0f },       // Sun (no tilt, or you can adjust for realism)
        { 1, 0.034f },   // Mercury
        { 2, 177.4f },   // Venus (retrograde)
        { 3, 23.44f },   // Earth
        { 4, 25.19f },   // Mars
        { 5, 3.13f },    // Jupiter
        { 6, 26.73f },   // Saturn
        { 7, 97.77f },   // Uranus (extreme tilt)
        { 8, 28.32f }    // Neptune
    };
    private Dictionary<int, Quaternion> precomputedRotations = new Dictionary<int, Quaternion>();
    public Dictionary<string, Dictionary<string, Vector3>> positionData = new Dictionary<string, Dictionary<string, Vector3>>();
    private Dictionary<int, List<Vector3>> animationPaths = new Dictionary<int, List<Vector3>>();
    private Dictionary<int, bool> planetAnimating = new Dictionary<int, bool>();
    
    private float elapsedTime = 0f; // Track the elapsed animation time
    private bool isAnimating = false;

    void Start()
    {
        LoadAllPlanetData();
        PrecomputePlanetRotations();
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
        TextAsset textAsset = Resources.Load<TextAsset>($"PlanetData/{dataDir}/planet_{objId}_positions");
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

                // Apply axial tilt
                if (precomputedRotations.TryGetValue(index, out Quaternion rotation))
                {
                    planets[index].transform.rotation = rotation;
                }
            }
            else
            {
                Debug.LogWarning($"No position data found for object {objId} on {targetDate}");
            }
        }
    }

    private string SimplifyHorizonsDate(string raw)
    {
        // raw = "A.D. 2023-Jan-01 00:00:00.0000 TDB"
        string trimmed = raw.Replace("A.D. ", "").Replace(" TDB", "").Trim();
        string[] parts = trimmed.Split(' ');
        string[] dateParts = parts[0].Split('-');

        string year = dateParts[0];
        string month = DateTime.ParseExact(dateParts[1], "MMM", CultureInfo.InvariantCulture).Month.ToString("D2");
        string day = dateParts[2];

        return $"{year}-{month}-{day}"; // format: yyyy-MM-dd
    }

    public void PrepareAnimationPaths()
    {
        animationPaths.Clear();
        planetAnimating.Clear();

        //DateTime start = ParseHorizonsDate(startDate);
        //DateTime end = ParseHorizonsDate(endDate);
        
        foreach (var obj in objectMapping)
        {
            int index = obj.Value;
            if (positionData.ContainsKey(obj.Key))
            {
                var clippedPath = positionData[obj.Key]
                .Where(kv =>
                {
                    string date = SimplifyHorizonsDate(kv.Key);
                    return string.Compare(date, startDate) >= 0 && string.Compare(date, endDate) <= 0;
                })
                .OrderBy(kv => SimplifyHorizonsDate(kv.Key))
                .Select(kv => kv.Value)
                .ToList();

                Debug.Log("Animation Days: " + clippedPath.Count);
                if (clippedPath.Count > 0)
                {
                    animationPaths[index] = clippedPath;
                    planetAnimating[index] = true;
                }
            }
        }
    }

    public void StartAnimation()
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

                    if (precomputedRotations.TryGetValue(index, out Quaternion rotation))
                    {
                        planets[index].transform.rotation = rotation;
                    }

               }
            }
        }
    }

    private Quaternion ComputeAxialRotation(int index, List<Vector3> positions, float axialTilt)
    {
        if (positions.Count < 2)
            return Quaternion.identity;

        Vector3 pos1 = positions[0];
        Vector3 pos2 = positions[positions.Count - 1];

        // Compute orbital plane normal
        Vector3 orbitNormal = Vector3.Cross(pos1, pos2).normalized;

        // Rotation from Unity up (Y axis) to orbit normal
        Quaternion orbitalPlaneRotation = Quaternion.FromToRotation(Vector3.up, orbitNormal);

        // Axial tilt relative to the orbital plane (assume local X as tilt axis)
        Quaternion axialTiltRotation = Quaternion.AngleAxis(axialTilt, Vector3.right);

        // Final combined rotation
        return orbitalPlaneRotation * axialTiltRotation;
    }

    void PrecomputePlanetRotations()
    {
        foreach (var obj in objectMapping)
        {
            int index = obj.Value;
            string objId = obj.Key;

            if (positionData.ContainsKey(objId))
            {
                var positions = positionData[objId].Values.ToList();

                if (positions.Count >= 2 && axialTiltDegrees.TryGetValue(index, out float tilt))
                {
                    Quaternion rotation = ComputeAxialRotation(index, positions, tilt);
                    precomputedRotations[index] = rotation;
                }
                else
                {
                    precomputedRotations[index] = Quaternion.identity;
                }
            }
        }
    }

}
