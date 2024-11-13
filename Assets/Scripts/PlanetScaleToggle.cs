using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetScaleToggle : MonoBehaviour
{
    public GameObject[] planets;           // Assign the planet GameObjects in the Inspector
    public Toggle ludicScaleToggle;        // Assign the Toggle UI element in the Inspector

    private Dictionary<GameObject, Vector3> realisticScales = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Vector3> ludicScales = new Dictionary<GameObject, Vector3>();
    private bool usingLudicScale = false;  // Track the current scale state

    void Start()
    {
        // Initialize scales
        InitializeScales();

        // Set up listener for the toggle
        ludicScaleToggle.onValueChanged.AddListener(OnToggleChanged);

        // Apply initial scale based on the toggle state
        ApplyScale(usingLudicScale);
    }

    // Initialize realistic and hardcoded ludic scales
    void InitializeScales()
    {
        // Store each planet's realistic scale
        foreach (var planet in planets)
        {
            realisticScales[planet] = planet.transform.localScale;
        }

        // Define hardcoded ludic scales for each planet
        for (int i = 0; i < planets.Length; i++)
        {
            GameObject planet = planets[i];
            Vector3 ludicScale;

            switch (planet.name.Replace("Model", ""))
            {
                case "Mercury":
                    ludicScale = new Vector3(741.76f, 741.76f, 741.76f);
                    break;
                case "Venus":
                    ludicScale = new Vector3(1839.20f, 1839.20f, 1839.20f);
                    break;
                case "Earth":
                    ludicScale = new Vector3(1936.48f, 1936.48f, 1936.48f);
                    break;
                case "Mars":
                    ludicScale = new Vector3(1030.56f, 1030.56f, 1030.56f);
                    break;
                case "Jupiter":
                    ludicScale = new Vector3(21252.63f, 21252.63f, 21252.63f);
                    break;
                case "Saturn":
                    ludicScale = new Vector3(17701.91f, 17701.91f, 17701.91f);
                    break;
                case "Uranus":
                    ludicScale = new Vector3(7709.44f, 7709.44f, 7709.44f);
                    break;
                case "Neptune":
                    ludicScale = new Vector3(304.00f, 304.00f, 304.00f);
                    break;
                default:
                    ludicScale = realisticScales[planet]; // Fallback to realistic scale if planet not listed
                    break;
            }

            ludicScales[planet] = ludicScale;
        }
    }

    // Method called when the toggle value changes
    void OnToggleChanged(bool isLudic)
    {
        usingLudicScale = isLudic;
        ApplyScale(usingLudicScale);
    }

    // Apply the selected scale to each planet
    void ApplyScale(bool useLudicScale)
    {
        string scaleReport = "Current Planet Scales:\n";

        foreach (var planet in planets)
        {
            Vector3 newScale = useLudicScale ? ludicScales[planet] : realisticScales[planet];
            planet.transform.localScale = newScale;
            scaleReport += $"{planet.name}: {newScale}\n";
        }

        Debug.Log(scaleReport); // Print consolidated report for easy copying
    }
}
