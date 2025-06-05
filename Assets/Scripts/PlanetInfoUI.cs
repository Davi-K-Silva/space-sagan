using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlanetInfoUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject hudRoot;                         // Root of all HUD elements
    public GameObject infoUiRoot;                      // InfoUI container (inside HUD)
    public GameObject planetInfoPanel;                 // Panel that holds dropdown + info label
    public Button openPlanetInfoButton;                // Button to toggle info panel
    public TMP_Dropdown planetDropdown;                // Planet selector dropdown
    public TextMeshProUGUI planetInfoLabel;            // Text label with planet data
    public TextMeshProUGUI planetHeaderLabel;          // Optional: header label that shows planet name

    [Header("Camera Scripts")]
    public PlanetLookCamera planetLookCamera;    // Lock mode camera
    public MonoBehaviour freeCameraScript;       // Free camera controller

    [Header("Planet Data")]
    public PlanetInfoLoader planetInfoLoader;          // Reference to data loader
    private List<PlanetInfo> planetData;               // Loaded data

    [Header("Planet Model Mapping")]
    public List<GameObject> planetModels; // Assign these in the inspector, exclude Sun

    private List<GameObject> otherHudElements = new List<GameObject>();
    private bool isInfoPanelVisible = false;

    void Start()
    {
        // Load data
        planetData = planetInfoLoader.LoadPlanetInfo();

        // Store all HUD siblings of InfoUI
        foreach (Transform child in hudRoot.transform)
        {
            if (child.gameObject != infoUiRoot)
                otherHudElements.Add(child.gameObject);
        }

        // Start hidden
        planetInfoPanel.SetActive(false);

        // Setup interactions
        openPlanetInfoButton.onClick.AddListener(TogglePlanetInfoPanel);
        planetDropdown.onValueChanged.AddListener(OnPlanetSelected);

        // Fill dropdown
        PopulateDropdown();
    }

    private GameObject GetPlanetObjectByName(string planetName)
    {
        foreach (GameObject obj in planetModels)
        {
            if (obj == null) continue;

            string cleanedName = obj.name.Replace("Model", "").Trim();
            if (cleanedName.Equals(planetName, System.StringComparison.OrdinalIgnoreCase))
            {
                return obj;
            }
        }

        Debug.LogWarning($"Planet model not found for {planetName}");
        return null;
    }

    void TogglePlanetInfoPanel()
    {
        isInfoPanelVisible = !isInfoPanelVisible;

        if (isInfoPanelVisible)
        {
            // Hide everything else, show planet panel
            foreach (GameObject go in otherHudElements)
                go.SetActive(false);

            planetInfoPanel.SetActive(true);
            planetDropdown.value = 0;
            OnPlanetSelected(0);
        }
        else
        {
            OnCameraLockToggle(false);
            planetLookCamera.SetOffsets(1f, 0.5f, 0.5f);  

            // Show normal HUD, hide planet panel
            foreach (GameObject go in otherHudElements)
                go.SetActive(true);

            planetInfoPanel.SetActive(false);
        }
    }

    void PopulateDropdown()
    {
        planetDropdown.ClearOptions();

        List<string> names = new List<string>();
        foreach (var planet in planetData)
            names.Add(planet.name);

        planetDropdown.AddOptions(names);
    }

    void OnPlanetSelected(int index)
    {
        PlanetInfo planet = planetData[index];

        string info = $"Mass: {planet.mass_10_24_kg} x10²⁴ kg\n" +
                      $"Diameter: {planet.diameter_km} km\n" +
                      $"Density: {planet.density_kg_m3} kg/m³\n" +
                      $"Gravity: {planet.gravity_m_s2} m/s²\n" +
                      $"Escape Velocity: {planet.escape_velocity_km_s} km/s\n" +
                      $"Rotation Period: {planet.rotation_period_hours} hrs\n" +
                      $"Day Length: {planet.length_of_day_hours} hrs\n" +
                      $"Distance from Sun: {planet.distance_from_sun_10_6_km} million km\n" +
                      $"Orbital Period: {planet.orbital_period_days} days\n" +
                      $"Orbital Speed: {planet.orbital_velocity_km_s} km/s\n" +
                      $"Orbital Inclination: {planet.orbital_inclination_deg}°\n" +
                      $"Obliquity: {planet.obliquity_deg}°\n" +
                      $"Mean Temp: {planet.mean_temperature_c} °C\n" +
                      $"Moons: {planet.number_of_moons}";

        planetInfoLabel.text = info;

        // Update header label
        if (planetHeaderLabel != null)
        {
            planetHeaderLabel.text = planet.name;
        }

        OnCameraLockToggle(true);

        GameObject planetObj = GetPlanetObjectByName(planet.name);
        GameObject sun = GetPlanetObjectByName("Sun");
        if (planetObj != null)
        {
            planetLookCamera.SetTargets(planetObj.transform, sun.transform);
            planetLookCamera.SetOffsets(1.2f, 0f, 0.7f); // Info view camera framing
        }

    }

    void OnCameraLockToggle(bool isLocked)
    {
        planetLookCamera.enabled = isLocked;
        freeCameraScript.enabled = !isLocked;
    }
}

