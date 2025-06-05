using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlanetInfo
{
    public string name;
    public float mass_10_24_kg;
    public int diameter_km;
    public int density_kg_m3;
    public float gravity_m_s2;
    public float escape_velocity_km_s;
    public float rotation_period_hours;
    public float length_of_day_hours;
    public float distance_from_sun_10_6_km;
    public float orbital_period_days;
    public float orbital_velocity_km_s;
    public float orbital_inclination_deg;
    public float obliquity_deg;
    public int mean_temperature_c;
    public int number_of_moons;
}

[System.Serializable]
public class PlanetInfoList
{
    public List<PlanetInfo> planets;
}

public class PlanetInfoLoader : MonoBehaviour
{
    public string jsonFileName = "planet_data.json";  // Place the file in StreamingAssets

    public List<PlanetInfo> LoadPlanetInfo()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (!File.Exists(path))
        {
            Debug.LogError("Planet info JSON not found at: " + path);
            return null;
        }

        string jsonContent = File.ReadAllText(path);

        // Wrap the JSON array for JsonUtility
        string wrappedJson = "{\"planets\":" + jsonContent + "}";

        PlanetInfoList list = JsonUtility.FromJson<PlanetInfoList>(wrappedJson);
        return list.planets;
    }
}

