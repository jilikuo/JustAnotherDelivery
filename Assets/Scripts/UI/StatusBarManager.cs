using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI dayText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;
    [SerializeField] private Button nextSceneButton;

    [SerializeField] private TimeObject currentTime;

    private void Awake()
    {
        if (dayText == null)
        {
            Debug.LogWarning("dayText not set");
        }
        if (timeText == null)
        {
            Debug.LogWarning("timeText not set");
        }
        if (moneyText == null)
        {
            Debug.LogWarning("moneyText not set");
        }
        if (nextSceneButton == null)
        {
            Debug.LogWarning("nextSceneButton not set");
        }
        nextSceneButton.onClick.AddListener(LoadNextScene);
        if (currentTime == null)
        {
            throw new System.Exception("Current Time object not set");
        }
    }

    private void LateUpdate()
    {
        dayText.GetComponent<TMPro.TextMeshProUGUI>().text = currentTime.dayString;
        timeText.GetComponent<TMPro.TextMeshProUGUI>().text = currentTime.timeString;
        moneyText.text = GameManager.instance.GetMoney().ToString();
    }

    public void LoadNextScene()
    {
        GameManager.instance.LoadNextScene();
    }
}
