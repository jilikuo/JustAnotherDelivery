using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyPanelManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;

    private void Start()
    {
        if (moneyText == null)
        {
            var go = gameObject.transform.gameObject.transform.Find("MoneyText");
            moneyText = go.GetComponent<TMPro.TextMeshProUGUI>();
            if (moneyText == null)
            {
                Debug.LogWarning("Money Text object not set");
            }
        }
    }

    private void Update()
    {
        moneyText.text = GameManager.instance.GetMoney().ToString();
    }
}
