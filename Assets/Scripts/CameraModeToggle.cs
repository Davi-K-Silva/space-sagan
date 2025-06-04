using UnityEngine;
using UnityEngine.UI;

public class CameraModeToggle : MonoBehaviour
{
    public Toggle modeToggle;  // Your UI toggle
    public MonoBehaviour freeCameraScript;     // Assign your free cam script
    public MonoBehaviour lockCameraScript;     // Assign the PlanetLookCamera script

    void Start()
    {
        modeToggle.onValueChanged.AddListener(OnToggleChanged);
        OnToggleChanged(modeToggle.isOn);  // Set initial state
    }

    void OnToggleChanged(bool isLocked)
    {
        freeCameraScript.enabled = !isLocked;
        lockCameraScript.enabled = isLocked;
    }
}

