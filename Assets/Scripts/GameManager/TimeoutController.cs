using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeoutController : MonoBehaviour
{
    [SerializeField] GameObject timeoutPanel;
    public void RestartDay()
    {
        timeoutPanel.SetActive(false);
        GameManager.instance.RestartDay();
    }
}
