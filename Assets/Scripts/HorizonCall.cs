using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class SolarSystemUpdater : MonoBehaviour
{
    private static readonly Quaternion tiltRotation = Quaternion.Euler(0, 0, 0);
    [SerializeField]
    private GameObject[] planets;  // Assign planet GameObjects in the Inspector

    public int scaleSpace;
    public int solarSystemSizeX;

    private string horizonsUrl = "https://ssd.jpl.nasa.gov/horizons_batch.cgi";

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

    void Start()
    {
        StartCoroutine(FetchDataForAllObjects());
    }

    IEnumerator FetchDataForAllObjects()
    {
        foreach (var obj in objectMapping)
        {
            string objId = obj.Key;
            yield return StartCoroutine(FetchHorizonsData(objId, "2023-01-01", "2024-01-01"));
        }
    }

    IEnumerator FetchHorizonsData(string objId, string startDate, string endDate)
    {
        string queryParams =
            $"?batch=1&MAKE_EPHEM=YES&COMMAND='{objId}'&EPHEM_TYPE='VECTORS'" +
            $"&CENTER='500@0'&START_TIME='{startDate}'&STOP_TIME='{endDate}'" +
            "&STEP_SIZE='1 DAYS'&VEC_TABLE='3'&REF_SYSTEM='ICRF'&REF_PLANE='F'" +
            "&VEC_CORR='NONE'&CAL_TYPE='M'&OUT_UNITS='KM-S'&VEC_LABELS='YES'" +
            "&VEC_DELTA_T='NO'&CSV_FORMAT='NO'&OBJ_DATA='YES'";

        using (UnityWebRequest request = UnityWebRequest.Get(horizonsUrl + queryParams))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string data = request.downloadHandler.text;

                // Parse dates and positions from the response data
                ParseEphemerisData(data, out List<string> dates, out List<Vector3> positions);

                // Save to a separate file for each planet
                SavePositionsToFile(objId, dates, positions);
            }
            else
            {
                Debug.LogError($"Error fetching data for {objId}: {request.error}");
            }
        }
    }

    void ParseEphemerisData(string data, out List<string> dates, out List<Vector3> positions)
    {
        dates = new List<string>();
        positions = new List<Vector3>();

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
                // Detect date lines
                if (line.Contains(" = A.D."))
                {
                    string datePart = line.Split('=')[1].Trim();
                    dates.Add(datePart);
                }
                else if (line.Trim().StartsWith("X"))
                {
                    // Adjust line if necessary, as in Python script
                    string adjustedLine = line.Replace("=-", "= -");
                    string[] parts = adjustedLine.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                    // Debug.Log(parts[0]);
                    // Debug.Log(parts[1]);
                    // Debug.Log(parts[2]);
                    // Debug.Log(parts[3]);
                    // Debug.Log(parts[4]);
                    // Debug.Log(parts[5]);
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[5]); //   \/    Changed  Y with Z  (Hotizon API Z  axis goes "UP" , so to match it with unity, it was changed to Y  )
                     float z = float.Parse(parts[3]); //  /\   Changed Z with Y

                    Vector3 position = new Vector3(x / scaleSpace, y / scaleSpace, z / scaleSpace);
                    positions.Add(position);
                }
            }
        }
        else
        {
            Debug.LogError("Invalid data format.");
        }
    }

    void SavePositionsToFile(string objId, List<string> dates, List<Vector3> positions)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"planet_{objId}_positions.txt");

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Date, X, Y, Z");

            for (int i = 0; i < positions.Count; i++)
            {
                string date = dates[i];
                Vector3 pos = positions[i];
                writer.WriteLine($"{date}, {pos.x}, {pos.y}, {pos.z}");
            }
        }

        Debug.Log($"Data for object {objId} saved to {filePath}");
    }
}
