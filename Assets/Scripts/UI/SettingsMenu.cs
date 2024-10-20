using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;

public class SettingsMenu : MonoBehaviour
{
    private GameObject child;

    public void Start()
    {
        // This object should have a single child
        child = transform.GetChild(0).gameObject;
        Show(false);
    }

    public void Show(bool show = true)
    {
        // This object should have a single child
        child.SetActive(show);
    }
}
