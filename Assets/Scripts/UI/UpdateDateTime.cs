using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpdateDateTime : MonoBehaviour
{
    public GameObject dateLabel;
    public GameObject timeLabel;

    public TimeObject currentTime;
    public BoolVariable isGamePaused;

    private void Awake()
    {
        if (dateLabel == null || timeLabel == null)
        {
            Debug.Log("Either Date Label or Time Label not found, the UI may not behave as expected");
        }
        if (currentTime == null)
        {
            throw new System.Exception("TimeObject not found, the game can't continue");
        }
        if (isGamePaused == null)
        {
            throw new System.Exception("isGamePaused Scriptable Object not found, the game can't continue");
        }
    }

    private void LateUpdate()
    {
        dateLabel.GetComponent<TMPro.TextMeshProUGUI>().text = currentTime.dayString;
        timeLabel.GetComponent<TMPro.TextMeshProUGUI>().text = currentTime.timeString;
    }

}
