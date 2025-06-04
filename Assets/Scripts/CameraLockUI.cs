using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraControlUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Dropdown fromDropdown;            // "From" planet
    public TMP_Dropdown toDropdown;              // "To" target
    public Toggle cameraLockToggle;              // Lock mode toggle
    public Button dropdownToggleButton;          // Button to show/hide the panel
    public GameObject dropdownPanel;             // The panel container

    [Header("Camera Scripts")]
    public PlanetLookCamera planetLookCamera;    // Lock mode camera
    public MonoBehaviour freeCameraScript;       // Free camera controller

    [Header("Celestial Bodies")]
    public List<Transform> celestialBodies;      // Planets, Sun, etc.

    private List<string> names = new List<string>();
    private bool isPanelVisible = false;

    void Start()
    {
        // Initially hide panel
        dropdownPanel.SetActive(false);
        isPanelVisible = false;

        // Setup dropdown button
        dropdownToggleButton.onClick.AddListener(ToggleDropdownPanel);

        // Populate dropdowns
        names.Clear();
        foreach (var body in celestialBodies)
        {
            names.Add(body.name.Replace("Model", "").Trim());
        }

        fromDropdown.ClearOptions();
        toDropdown.ClearOptions();
        fromDropdown.AddOptions(names);
        toDropdown.AddOptions(names);

        // Default selections (optional)
        fromDropdown.value = Mathf.Clamp(names.FindIndex(n => n.ToLower().Contains("earth")), 0, names.Count - 1);
        toDropdown.value = Mathf.Clamp(names.FindIndex(n => n.ToLower().Contains("sun")), 0, names.Count - 1);

        // Register UI event listeners
        fromDropdown.onValueChanged.AddListener(delegate { UpdateCameraTargets(); });
        toDropdown.onValueChanged.AddListener(delegate { UpdateCameraTargets(); });
        cameraLockToggle.onValueChanged.AddListener(OnCameraLockToggle);

        // Initial setup
        UpdateCameraTargets();
        OnCameraLockToggle(cameraLockToggle.isOn);
    }

    void ToggleDropdownPanel()
    {
        isPanelVisible = !isPanelVisible;
        dropdownPanel.SetActive(isPanelVisible);
    }

    void UpdateCameraTargets()
    {
        if (celestialBodies.Count == 0) return;

        var fromP= celestialBodies[fromDropdown.value];
        var toP = celestialBodies[toDropdown.value];

        planetLookCamera.planetToFollow = fromP;
        planetLookCamera.targetToLookAt = toP;
    }

    void OnCameraLockToggle(bool isLocked)
    {
        planetLookCamera.enabled = isLocked;
        freeCameraScript.enabled = !isLocked;
    }
}

