using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Optional fields")]
    [SerializeField] private TimeSystem timeSystem;
    [SerializeField] private GameObject timeoutPanelParent;

    private void Start()
    {
        if (timeoutPanelParent == null)
        {
            timeoutPanelParent = GameObject.FindGameObjectWithTag("TimeoutPanel");
        }
        if (timeSystem == null)
        {
            timeSystem = GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>();
        }
        timeSystem.timeoutEvent.AddListener(ShowTimeoutPanel);
    }

    public void LoadNextScene()
    {
        GameManager.instance.LoadNextScene();
    }

    public void RestartDay()
    { 
        GameManager.instance.RestartDay();
    }

    private void ShowTimeoutPanel()
    {
        if (timeoutPanelParent != null)
        {
            Debug.Log("Ran out of time");
            // timeoutPanelParent is enabled to make it Findable.
            // It has one child - the actual panel, which is disabled.
            timeoutPanelParent.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
