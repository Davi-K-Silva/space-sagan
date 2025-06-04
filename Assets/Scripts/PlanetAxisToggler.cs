using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetAxisToggler : MonoBehaviour
{
    [Tooltip("Toggle to enable or disable axis visibility.")]
    public Toggle axisToggle;

    [Tooltip("List of GameObjects representing each planet's axis (e.g., cylinders or arrows).")]
    public List<GameObject> axisObjects;

    void Start()
    {
        if (axisToggle != null)
        {
            axisToggle.onValueChanged.AddListener(OnToggleChanged);
            OnToggleChanged(axisToggle.isOn); // Set initial state
        }
    }

    void OnDestroy()
    {
        if (axisToggle != null)
        {
            axisToggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }

    void OnToggleChanged(bool isOn)
    {
        foreach (GameObject axis in axisObjects)
        {
            if (axis != null)
                axis.SetActive(isOn);
        }
    }
}

