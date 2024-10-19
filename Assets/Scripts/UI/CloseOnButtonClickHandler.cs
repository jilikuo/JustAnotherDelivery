using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloseOnButtonClickHandler : CloseOnEscHandler
{
    Button button;

    private void Start()
    {
        var buttons = gameObject.GetComponentsInChildren<Button>();
        button = buttons.Last();
        button.onClick.AddListener(ClosePanel);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
