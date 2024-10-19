using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleChildOnKeyController : MonoBehaviour
{
    [SerializeField] protected KeyCode keyCode;
    protected GameObject childPanel;

    private void Start()
    {
        // It has one child - the actual panel, which is disabled.
        childPanel = gameObject.transform.GetChild(0).gameObject;
        OnStart();
    }

    protected virtual void OnStart()
    {
    }

    private void Update()
    {
        if (GameManager.instance.IsGamePaused() && !childPanel.activeSelf)
        {
            return;
        }
        if (Input.GetKeyUp(keyCode))
        {
            childPanel.SetActive(!childPanel.activeSelf);
            OnUpdate();
        }
    }

    protected virtual void OnUpdate()
    {
    }
}
