using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayTimePanelManager : MonoBehaviour
{
    public GameObject dayText;
    public GameObject timeText;

    public TimeObject currentTime;

    private void Awake()
    {
        if (dayText == null)
        {
            Debug.LogWarning("Day Text object not set");
        }
        if (timeText == null)
        {
            Debug.LogWarning("Time Text object not set");
        }
        if (currentTime == null)
        {
            throw new System.Exception("Current Time object not set");
        }
    }

    private void LateUpdate()
    {
        dayText.GetComponent<TMPro.TextMeshProUGUI>().text = currentTime.dayString;
        timeText.GetComponent<TMPro.TextMeshProUGUI>().text = currentTime.timeString;
    }

    public void LoadNextScene()
    {
        GameManager.instance.LoadNextScene();
    }
}
