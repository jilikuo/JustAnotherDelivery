using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class SettingsMenu : MonoBehaviour
{
    private GameObject child;

    private void Start()
    {
        // This object should have a single child
        child = transform.GetChild(0).gameObject;
        Show(false);
    }

    public void Show(bool show = true)
    {
        child.SetActive(show);
    }
}
