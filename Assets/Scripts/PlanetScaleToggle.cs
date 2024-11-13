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
        // Assign each planet's ludic scale individually
        for (int i = 0; i < planets.Length; i++)
        {
            GameObject planet = planets[i];
            Vector3 ludicScale;

            switch (planet.name)
            {
                case "Sun":
                    ludicScale = new Vector3(50, 50, 50); // Example ludic scale for the Sun
                    break;
                case "Mercury":
                    ludicScale = new Vector3(1, 1, 1); // Example ludic scale for Mercury
                    break;
                case "Venus":
                    ludicScale = new Vector3(2, 2, 2); // Example ludic scale for Venus
                    break;
                case "Earth":
                    ludicScale = new Vector3(2.5f, 2.5f, 2.5f); // Example ludic scale for Earth
                    break;
                case "Mars":
                    ludicScale = new Vector3(1.5f, 1.5f, 1.5f); // Example ludic scale for Mars
                    break;
                case "Jupiter":
                    ludicScale = new Vector3(10, 10, 10); // Example ludic scale for Jupiter
                    break;
                case "Saturn":
                    ludicScale = new Vector3(9, 9, 9); // Example ludic scale for Saturn
                    break;
                case "Uranus":
                    ludicScale = new Vector3(4, 4, 4); // Example ludic scale for Uranus
                    break;
                case "Neptune":
                    ludicScale = new Vector3(3.5f, 3.5f, 3.5f); // Example ludic scale for Neptune
                    break;
                default:
                    ludicScale = realisticScales[planet]; // Fallback to realistic scale
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
