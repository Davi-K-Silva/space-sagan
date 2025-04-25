using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationSettingsUI : MonoBehaviour
{
    public SolarSystemLoader solarSystemLoader;    // Reference to the SolarSystemLoader script

    // Start Date dropdowns
    public TMP_Dropdown startYearDropdown;
    public TMP_Dropdown startMonthDropdown;
    public TMP_Dropdown startDayDropdown;

    // End Date dropdowns
    public TMP_Dropdown endYearDropdown;
    public TMP_Dropdown endMonthDropdown;
    public TMP_Dropdown endDayDropdown;

    // Animation Time dropdown
    public TMP_Dropdown animationTimeDropdown;

    // Buttons and Panel
    public Button animateButton;
    public Button toggleButton;
    public GameObject settingsPanel;

    private bool isPanelVisible = false;

    void Start()
    {
        settingsPanel.SetActive(false);  // Hide the panel initially

        // Add listeners
        animateButton.onClick.AddListener(OnAnimateButtonClicked);
        toggleButton.onClick.AddListener(ToggleSettingsPanel);

        // Initialize dropdowns
        InitializeYearDropdown(startYearDropdown);
        InitializeYearDropdown(endYearDropdown);
        InitializeMonthDropdown(startMonthDropdown);
        InitializeMonthDropdown(endMonthDropdown);
        UpdateDayDropdown(startYearDropdown, startMonthDropdown, startDayDropdown);
        UpdateDayDropdown(endYearDropdown, endMonthDropdown, endDayDropdown);
        InitializeAnimationTimeDropdown();

        // Add listeners to update days when month or year changes
        startYearDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(startYearDropdown, startMonthDropdown, startDayDropdown); });
        startMonthDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(startYearDropdown, startMonthDropdown, startDayDropdown); });
        endYearDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(endYearDropdown, endMonthDropdown, endDayDropdown); });
        endMonthDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(endYearDropdown, endMonthDropdown, endDayDropdown); });
    }

    void ToggleSettingsPanel()
    {
        isPanelVisible = !isPanelVisible;
        settingsPanel.SetActive(isPanelVisible);
    }

    void InitializeYearDropdown(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        var years = new System.Collections.Generic.List<string>();
        for (int year = 2000; year <= 2024; year++)
        {
            years.Add(year.ToString());
        }
        dropdown.AddOptions(years);
    }

    void InitializeMonthDropdown(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();
        var months = new System.Collections.Generic.List<string>
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };
        dropdown.AddOptions(months);
    }

    void UpdateDayDropdown(TMP_Dropdown yearDropdown, TMP_Dropdown monthDropdown, TMP_Dropdown dayDropdown)
    {
        dayDropdown.ClearOptions();
        int selectedYear = int.Parse(yearDropdown.options[yearDropdown.value].text);
        int selectedMonth = monthDropdown.value + 1;  // Convert to 1-based month

        int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
        var days = new System.Collections.Generic.List<string>();
        for (int day = 1; day <= daysInMonth; day++)
        {
            days.Add(day.ToString());
        }
        dayDropdown.AddOptions(days);
    }

    void InitializeAnimationTimeDropdown()
    {
        animationTimeDropdown.ClearOptions();
        var times = new System.Collections.Generic.List<string> { "20", "40", "60" };
        animationTimeDropdown.AddOptions(times);
    }

    void OnAnimateButtonClicked()
    {
        // Get Start Date
        string startYear = startYearDropdown.options[startYearDropdown.value].text;
        string startMonth = (startMonthDropdown.value + 1).ToString("D2");  // Zero-padded month
        string startDay = startDayDropdown.options[startDayDropdown.value].text.PadLeft(2, '0');  // Zero-padded day
        string startDate = $"{startYear}-{startMonth}-{startDay}";

        // Get End Date
        string endYear = endYearDropdown.options[endYearDropdown.value].text;
        string endMonth = (endMonthDropdown.value + 1).ToString("D2");
        string endDay = endDayDropdown.options[endDayDropdown.value].text.PadLeft(2, '0');
        string endDate = $"{endYear}-{endMonth}-{endDay}";

        // Get Animation Time
        float animationTime = float.Parse(animationTimeDropdown.options[animationTimeDropdown.value].text);

        // Pass values to SolarSystemLoader
        solarSystemLoader.startDate = startDate;
        solarSystemLoader.endDate = endDate;
        solarSystemLoader.maxAnimationTime = animationTime;
        solarSystemLoader.PrepareAnimationPaths();  // Set up paths based on the start date
        solarSystemLoader.StartAnimation();         // Start animating

        Debug.Log($"Animation from {startDate} to {endDate} over {animationTime} seconds");

        settingsPanel.SetActive(false);  // Hide panel after starting animation
        isPanelVisible = false;
    }
}
