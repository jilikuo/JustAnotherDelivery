using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloseOnEscHandler : MonoBehaviour
{ 
    private void OnEnable()
    {
        GameManager.instance.AddEscMenu(gameObject);
    }

    private void OnDisable()
    {
        GameManager.instance.RemoveEscMenu(gameObject);
    }
}
