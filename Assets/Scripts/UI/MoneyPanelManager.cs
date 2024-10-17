using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyPanelManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (moneyText == null)
        {
            Debug.LogWarning("Money Text object not set");
        }
    }

    private void Start()
    {
        moneyText = moneyText.GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        moneyText.text = GameManager.instance.GetMoney().ToString();
    }
}
