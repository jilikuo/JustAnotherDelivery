using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpController : MonoBehaviour
{
    private GameObject helpPanel;
    private Toggle helpToggle;
    private string alwaysShowHelpKey = "alwaysShowHelp";

    private void Start()
    {
        // It has one child - the actual panel, which is disabled.
        helpPanel = gameObject.transform.GetChild(0).gameObject;
        helpToggle = helpPanel.GetComponentInChildren<Toggle>();

        bool alwaysShowHelp = PlayerPrefs.GetInt(alwaysShowHelpKey, 1) == 1;
        helpToggle.isOn = alwaysShowHelp;
        helpPanel.SetActive(alwaysShowHelp);
    }

    private void Update()
    {
        if (GameManager.instance.IsGamePaused() && !helpPanel.activeSelf)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            if (helpPanel.activeSelf)
            {
                helpPanel.SetActive(false);
                PlayerPrefs.SetInt(alwaysShowHelpKey, (helpToggle.isOn) ? 1 : 0);
                GameManager.instance.UnpauseGame();
            }
            else
            {
                GameManager.instance.PauseGame();
                helpPanel.SetActive(true);
            }
        }
    }
}
