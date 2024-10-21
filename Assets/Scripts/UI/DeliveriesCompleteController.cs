using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliveriesCompleteController : MonoBehaviour
{
    private GameObject deliveriesCompletePanel;

    private void Start()
    {
        // It has one child - the actual panel, which is disabled.
        deliveriesCompletePanel = gameObject.transform.GetChild(0).gameObject;
    }

    public void ShowDeliveriesCompletePanel()
    {
        if (!deliveriesCompletePanel.activeSelf)
        {
            Debug.Log("Completed deliveries");
        }
        var timeSystem = GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>();
        timeSystem.StopTime();
        deliveriesCompletePanel.SetActive(true);
    }

    public void Continue()
    {
        deliveriesCompletePanel.SetActive(false);
        GameManager.instance.LoadScene(GameManager.GameScene.UpgradeMenuScene);
    }
}
