using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpMenuHanlder : ToggleChildOnKeyController
{
    protected override void OnStart()
    {
        keyCode = KeyCode.H;

        childPanel.SetActive(GameManager.instance.IsAlwaysShowHelp());
    }
}
