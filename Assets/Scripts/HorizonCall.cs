using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SolarSystemUpdater : MonoBehaviour
{
    [SerializeField]
    private GameObject[] planets;  // Assign planet GameObjects in the Inspector

    public int scaleSpace;
    public int solarSystemSizeX;

    private string horizonsUrl = "https://ssd.jpl.nasa.gov/horizons_batch.cgi";

    // Object ID to GameObject mapping (ID to index)
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchDataForAllObjects());
    }

    // Fetch data for all objects in the mapping
    IEnumerator FetchDataForAllObjects()
    {
        foreach (var obj in objectMapping)
        {
            string objId = obj.Key;
            yield return StartCoroutine(FetchHorizonsData(objId, "2023-01-01", "2024-01-01"));
        }
    }

    // Coroutine to fetch ephemeris data for a specific object
    IEnumerator FetchHorizonsData(string objId, string startDate, string endDate)
    {
        string queryParams =
            $"?batch=1&MAKE_EPHEM=YES&COMMAND='{objId}'&EPHEM_TYPE='VECTORS'" +
            $"&CENTER='500@0'&START_TIME='{startDate}'&STOP_TIME='{endDate}'" +
            "&STEP_SIZE='1 DAYS'&VEC_TABLE='3'&REF_SYSTEM='ICRF'&REF_PLANE='ECLIPTIC'" +
            "&VEC_CORR='NONE'&CAL_TYPE='M'&OUT_UNITS='KM-S'&VEC_LABELS='YES'" +
            "&VEC_DELTA_T='NO'&CSV_FORMAT='NO'&OBJ_DATA='YES'";

        using (UnityWebRequest request = UnityWebRequest.Get(horizonsUrl + queryParams))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string data = request.downloadHandler.text;
                Vector3 position = ParseEphemerisData(data);
                UpdatePlanetPosition(objId, position);
            }
            else
            {
                Debug.LogError($"Error fetching data for {objId}: {request.error}");
            }
        }
    }

    // Parse the Horizons data to extract the first X, Y, Z position
    Vector3 ParseEphemerisData(string data)
    {
        string startMarker = "$$SOE";
        string endMarker = "$$EOE";
        int startIndex = data.IndexOf(startMarker) + startMarker.Length;
        int endIndex = data.IndexOf(endMarker);

        if (startIndex < endIndex)
        {
            string ephemerisData = data.Substring(startIndex, endIndex - startIndex).Trim();
            string[] lines = ephemerisData.Split('\n');

            foreach (string line in lines)
            {
                if (line.Contains("X ="))
                {
                    string[] parts = line.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                     Debug.Log($"{parts[0]} <> {parts[1]} <> {parts[3]} <> {parts}");
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[3]);
                    float z = float.Parse(parts[5]);
                    Debug.Log($"X: {x} Y: {y} Z:{z}");
                    Debug.Log($"[X: {x/scaleSpace} Y: {y/scaleSpace} Z:{z/scaleSpace}]");
                    return new Vector3(x/scaleSpace, y/scaleSpace, z/scaleSpace);  // Return the first position found
                }
            }
        }

        Debug.LogError("Invalid data format.");
        return Vector3.zero;  // Default if parsing fails
    }

    // Update the position of the corresponding planet GameObject
    void UpdatePlanetPosition(string objId, Vector3 position)
    {
        if (objectMapping.TryGetValue(objId, out int index) && index < planets.Length)
        {
            planets[index].transform.position = position;
            // planets[index].transform.localScale = new Vector3(planets[index].transform.localScale.x/1000000,
            //                                                   planets[index].transform.localScale.y/1000000,
            //                                                   planets[index].transform.localScale.z/1000000);
        }
        else
        {
            Debug.LogError($"No GameObject assigned for object ID {objId}");
        }
    }
}
