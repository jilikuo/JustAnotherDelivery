using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject moneyText;

    private void Awake()
    {
        if (moneyText == null)
        {
            Debug.LogWarning("Money Text object not set");
        }
    }

    private void Update()
    {
        moneyText.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.instance.GetMoney().ToString();
    }
}
