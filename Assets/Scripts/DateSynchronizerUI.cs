using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMesh Pro support

public class DateSynchronizerUI : MonoBehaviour
{
    public SolarSystemLoader solarSystemLoader;  // Reference to SolarSystemLoader script
    public TMP_InputField dateInputField;        // TextMesh Pro Input Field for date entry
    public Button syncButton;                    // Button to synchronize positions
    public Button dropdownToggleButton;          // Button to open/close the dropdown
    public GameObject dropdownPanel;             // The dropdown panel to show/hide

    private bool isPanelVisible = false;         // Track panel visibility

    void Start()
    {
        // Hide the dropdown panel initially
        dropdownPanel.SetActive(false);

        // Add button listeners
        syncButton.onClick.AddListener(OnSyncButtonClicked);
        dropdownToggleButton.onClick.AddListener(ToggleDropdownPanel);
    }

    // Toggle dropdown panel visibility
    void ToggleDropdownPanel()
    {
        isPanelVisible = !isPanelVisible;
        dropdownPanel.SetActive(isPanelVisible);
    }

    // Synchronize planets to the specified date when Sync button is clicked
    void OnSyncButtonClicked()
    {
        string date = "A.D. " + dateInputField.text + " 00:00:00.0000 TDB";  // Get the date from the TMP input field

        // Check if SolarSystemLoader instance and data are available
        if (solarSystemLoader != null && solarSystemLoader.positionData != null)
        {
            // Set the target date and update planet positions
            solarSystemLoader.targetDate = date;
            solarSystemLoader.UpdatePlanetsToTargetDate();

            // Optionally, hide the dropdown after syncing
            dropdownPanel.SetActive(false);
            isPanelVisible = false;
        }
        else
        {
            Debug.LogError("SolarSystemLoader instance or data not found.");
        }
    }
}
