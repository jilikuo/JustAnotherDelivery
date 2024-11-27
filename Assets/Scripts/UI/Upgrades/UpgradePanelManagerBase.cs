using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradePanelManagerBase : MonoBehaviour
{
    [SerializeField] private int cost;

    [SerializeField] protected GameObject currentStateDisplayPanel;
    [SerializeField] protected TMPro.TextMeshProUGUI upgradeTypeText;
    [SerializeField] protected TMPro.TextMeshProUGUI currentStateText;

    [SerializeField] protected GameObject nextStateDisplayPanel;
    [SerializeField] protected TMPro.TextMeshProUGUI nextStateText;

    [SerializeField] protected TMPro.TextMeshProUGUI upgradeBuyText;
    [SerializeField] protected TMPro.TextMeshProUGUI upgradeCostText;
    [SerializeField] protected Button upgradeButton;

    protected virtual void SetVars()
    {
        currentStateDisplayPanel = gameObject.transform.Find("CurrentStateDisplayPanel").gameObject;
        currentStateText = currentStateDisplayPanel.transform.Find("CurrentStateText").GetComponent<TMPro.TextMeshProUGUI>();
        upgradeTypeText = currentStateDisplayPanel.transform.Find("TypeText").GetComponent<TMPro.TextMeshProUGUI>();
        upgradeTypeText.text = GetUpgradeLabel() + ":";

        nextStateDisplayPanel = gameObject.transform.Find("NextStateDisplayPanel").gameObject;
        nextStateText = nextStateDisplayPanel.transform.Find("UpgradeLabel/NextStateText").GetComponent<TMPro.TextMeshProUGUI>();

        upgradeButton = nextStateDisplayPanel.transform.Find("UpgradeButton").GetComponent<Button>();
        upgradeButton.onClick.AddListener(BuyUpgrade);
        upgradeBuyText = upgradeButton.transform.Find("BuyText").GetComponent<TMPro.TextMeshProUGUI>();
        upgradeCostText = upgradeButton.transform.Find("CostText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        if ((cost != GetUpgradeCost()) || HasUpdatedValue())
        {
            UpdateUpgradePanel();
        }
        if (cost < 0)
        {
            DimText(upgradeTypeText, true);
            upgradeCostText.text = "";
            EnableUpgradePanel(false);
        }
        else
        {
            EnableUpgradePanel(cost <= GameManager.instance.GetMoney());
        }
    }

    protected virtual void EnableUpgradePanel(bool enable)
    {
        DimText(nextStateText, !enable);
        DimText(upgradeBuyText, !enable);
        DimText(upgradeCostText, !enable);
        upgradeButton.enabled = enable;
    }

    protected void DimText(TextMeshProUGUI text, bool dim)
    {
        var color = text.color;
        color.a = (dim) ? .42f : 1.0f;
        text.color = color;
    }

    private void BuyUpgrade()
    {
        if (GameManager.instance.SpendMoney(cost))
        {
            DoUpgrade();
            UpdateUpgradePanel();
        }
        else
        {
            Debug.Log("Failed to buy upgrade - Cost: " + cost + " Available: " + GameManager.instance.GetMoney());
        }
    }

    protected void UpdateUpgradePanel()
    {
        UpdateValues();
        cost = GetUpgradeCost();
        upgradeCostText.text = cost.ToString();
        UpdateDisplay();
    }

    protected abstract string GetUpgradeLabel();
    protected abstract int GetUpgradeCost();
    protected abstract bool HasUpdatedValue();
    protected abstract void UpdateValues();
    protected abstract void UpdateDisplay();
    protected abstract void DoUpgrade();
}
