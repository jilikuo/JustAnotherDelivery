using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpController : ToggleChildOnKeyController
{
    private Toggle helpToggle;
    private string alwaysShowHelpKey = "alwaysShowHelp";

    protected override void OnStart()
    {
        keyCode = KeyCode.H;

        helpToggle = childPanel.GetComponentInChildren<Toggle>();

        bool alwaysShowHelp = PlayerPrefs.GetInt(alwaysShowHelpKey, 1) == 1;
        helpToggle.isOn = alwaysShowHelp;
        childPanel.SetActive(alwaysShowHelp);
    }

    protected override void OnUpdate()
    {
        PlayerPrefs.SetInt(alwaysShowHelpKey, (helpToggle.isOn) ? 1 : 0);
    }
}
