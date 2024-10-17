using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeoutController : MonoBehaviour
{
    private GameObject timeoutPanel;

    private void Start()
    {
        var timeSystem = GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>();
        timeSystem.timeoutEvent.AddListener(ShowTimeoutPanel);
        // It has one child - the actual panel, which is disabled.
        timeoutPanel = gameObject.transform.GetChild(0).gameObject;
    }

    private void ShowTimeoutPanel()
    {
        Debug.Log("Ran out of time");
        timeoutPanel.SetActive(true);
    }

    public void RestartDay()
    {
        timeoutPanel.SetActive(false);
        GameManager.instance.RestartDay();
    }
}
