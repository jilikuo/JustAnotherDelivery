using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject timeoutPanel;

    public void LoadNextScene()
    {
        GameManager.instance.LoadNextScene();
    }

    void OnEnable()
    {
        if (timeoutPanel != null)
        {
            timeoutPanel.SetActive(false);
            GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>().timeoutEvent.AddListener(ShowTimeoutPanel);
        }
    }

    void OnDisable()
    {
        if (timeoutPanel != null)
        {
            GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>().timeoutEvent.RemoveListener(ShowTimeoutPanel);
        }
    }

    private void ShowTimeoutPanel()
    {
        Debug.Log("Ran out of time");
        timeoutPanel.SetActive(true);
    }
}
