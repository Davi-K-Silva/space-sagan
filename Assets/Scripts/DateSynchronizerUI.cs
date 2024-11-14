using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DateSynchronizerUI: MonoBehaviour
{
    public SolarSystemLoader solarSystemLoader;      // Reference to SolarSystemLoader script
    public TMP_Dropdown yearDropdown;                // Year dropdown
    public TMP_Dropdown monthDropdown;               // Month dropdown
    public TMP_Dropdown dayDropdown;                 // Day dropdown
    public Button syncButton;                        // Button to sync planets
    public Button dropdownToggleButton;              // Button to open/close the date panel
    public GameObject dropdownPanel;                 // The dropdown panel

    private bool isPanelVisible = false;

    void Start()
    {
        dropdownPanel.SetActive(false); // Hide panel initially

        syncButton.onClick.AddListener(OnSyncButtonClicked);
        dropdownToggleButton.onClick.AddListener(ToggleDropdownPanel);

        InitializeYearDropdown();
        InitializeMonthDropdown();
        UpdateDayDropdown();
        
        // Add listeners to update days when month or year changes
        yearDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });
        monthDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });
    }

    void ToggleDropdownPanel()
    {
        isPanelVisible = !isPanelVisible;
        dropdownPanel.SetActive(isPanelVisible);
    }

    void InitializeYearDropdown()
    {
        yearDropdown.ClearOptions();
        var years = new System.Collections.Generic.List<string>();
        for (int year = 2000; year <= 2024; year++)
        {
            years.Add(year.ToString());
        }
        yearDropdown.AddOptions(years);
    }

    void InitializeMonthDropdown()
    {
        monthDropdown.ClearOptions();
        var months = new System.Collections.Generic.List<string>
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };
        monthDropdown.AddOptions(months);
    }

    void UpdateDayDropdown()
    {
        dayDropdown.ClearOptions();

        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedMonth = monthDropdown.value + 1; // Convert to 1-based month

        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        var days = new System.Collections.Generic.List<string>();
        for (int day = 1; day <= daysInMonth; day++)
        {
            days.Add(day.ToString());
        }
        dayDropdown.AddOptions(days);
    }

    void OnSyncButtonClicked()
    {
        // Construct the date in YYYY-MM-DD format
        string year = yearDropdown.options[yearDropdown.value].text;
        string month = monthDropdown.options[monthDropdown.value].text; // Zero-padded month
        string day = dayDropdown.options[dayDropdown.value].text.PadLeft(2, '0'); // Zero-padded day

        string selectedDate = $"A.D. {year}-{month}-{day} 00:00:00.0000 TDB";
        Debug.Log(selectedDate);
        solarSystemLoader.targetDate = selectedDate;
        solarSystemLoader.UpdatePlanetsToTargetDate();

        dropdownPanel.SetActive(false); // Optionally close the panel after syncing
        isPanelVisible = false;
    }
}
