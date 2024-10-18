using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradePanelManagerBase : MonoBehaviour
{
    [SerializeField] private float cost;

    [SerializeField] protected GameObject currentStateDisplayPanel;
    [SerializeField] protected Image currentStateBackground;
    [SerializeField] protected TMPro.TextMeshProUGUI currentStateCenterText;
    [SerializeField] protected TMPro.TextMeshProUGUI currentStateBottomText;

    [SerializeField] protected GameObject nextStateDisplayPanel;
    [SerializeField] protected Image nextStateBackground;
    [SerializeField] protected TMPro.TextMeshProUGUI nextStateCenterText;
    [SerializeField] protected TMPro.TextMeshProUGUI nextStateBottomText;

    [SerializeField] protected GameObject upgradeTermsPanel;
    [SerializeField] protected TMPro.TextMeshProUGUI upgradeLabelText;
    [SerializeField] protected TMPro.TextMeshProUGUI upgradeCostText;
    [SerializeField] protected Button upgradeButton;

    protected virtual void SetVars()
    {
        currentStateDisplayPanel = gameObject.transform.Find("CurrentStateDisplayPanel").gameObject;
        currentStateBackground = currentStateDisplayPanel.GetComponent<Image>();
        currentStateCenterText = currentStateDisplayPanel.transform.Find("CenterInfoText").GetComponent<TMPro.TextMeshProUGUI>();
        currentStateBottomText = currentStateDisplayPanel.transform.Find("BottomInfoText").GetComponent<TMPro.TextMeshProUGUI>();

        nextStateDisplayPanel = gameObject.transform.Find("NextStateDisplayPanel").gameObject;
        nextStateBackground = nextStateDisplayPanel.GetComponent<Image>();
        nextStateCenterText = nextStateDisplayPanel.transform.Find("CenterInfoText").GetComponent<TMPro.TextMeshProUGUI>();
        nextStateBottomText = nextStateDisplayPanel.transform.Find("BottomInfoText").GetComponent<TMPro.TextMeshProUGUI>();

        upgradeTermsPanel = gameObject.transform.Find("UpgradeTermsPanel").gameObject;
        upgradeLabelText = upgradeTermsPanel.transform.Find("TypeText").GetComponent<TMPro.TextMeshProUGUI>();
        upgradeLabelText.text = GetUpgradeLabel();
        upgradeCostText = upgradeTermsPanel.transform.Find("CostText").GetComponent<TMPro.TextMeshProUGUI>();
        upgradeButton = upgradeTermsPanel.transform.Find("UpgradeButton").GetComponent<Button>();
        upgradeButton.onClick.AddListener(BuyUpgrade);
    }

    private void LateUpdate()
    {
        if ((cost != GetUpgradeCost()) || HasUpdatedValue())
        {
            UpdateUpgradePanel();
        }
        if (cost < 0)
        {
            upgradeLabelText.enabled = false;
            upgradeCostText.text = "";
            upgradeCostText.enabled = false;
            upgradeButton.enabled = false;
        }
        else
        {
            upgradeButton.enabled = (cost <= GameManager.instance.GetMoney());
        }
    }

    private void BuyUpgrade()
    {
        if (GameManager.instance.SpendMoney(cost))
        {
            DoUpgrade();
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
        upgradeCostText.text = "Cost: " + cost.ToString();
        UpdateDisplay();
    }

    protected abstract string GetUpgradeLabel();
    protected abstract float GetUpgradeCost();
    protected abstract bool HasUpdatedValue();
    protected abstract void UpdateValues();
    protected abstract void UpdateDisplay();
    protected abstract void DoUpgrade();
}
