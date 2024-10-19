using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.instance.PauseGame();
    }

    private void OnDisable()
    {
        GameManager.instance.UnpauseGame();
    }
}
