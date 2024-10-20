using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowHelpOnToggleHandler : MonoBehaviour
{
    private Toggle showHelpToggle;

    private void OnEnable()
    {
        showHelpToggle = gameObject.GetComponentInChildren<Toggle>();
        showHelpToggle.isOn = GameManager.instance.IsAlwaysShowHelp();
        showHelpToggle.onValueChanged.AddListener(UpdateHelp);
    }

    private void OnDisable()
    {
        showHelpToggle.onValueChanged.RemoveListener(UpdateHelp);
        GameManager.instance.SetAlwaysShowHelp(showHelpToggle.isOn);
    }

    private void UpdateHelp(bool isSet)
    {
        GameManager.instance.SetAlwaysShowHelp(isSet);
    }
}
