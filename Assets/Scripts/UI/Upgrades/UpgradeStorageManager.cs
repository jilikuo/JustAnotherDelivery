using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStorageManager : MonoBehaviour
{   enum UpgradeType
    {
        None,
        ExpandMessengerBag,
        AddFrontBasket,
        AddRearBasket,
        AddSaddlebags,
        NumUpgradeTypes
    };
    static Dictionary<UpgradeType, InventoryUpgradeDisplayManager.InventoryContainer> upgradeTypeToContainer =
    new Dictionary<UpgradeType, InventoryUpgradeDisplayManager.InventoryContainer>() {
            {UpgradeType.None, InventoryUpgradeDisplayManager.InventoryContainer.None},
            {UpgradeType.ExpandMessengerBag, InventoryUpgradeDisplayManager.InventoryContainer.NextMessengerBag},
            {UpgradeType.AddFrontBasket, InventoryUpgradeDisplayManager.InventoryContainer.FrontBasket},
            {UpgradeType.AddRearBasket, InventoryUpgradeDisplayManager.InventoryContainer.RearBasket},
            {UpgradeType.AddSaddlebags,InventoryUpgradeDisplayManager.InventoryContainer.SaddleBags }
        };

    [SerializeField] private TMPro.TextMeshProUGUI upgradeCostText;
    [SerializeField] private TMPro.TMP_Dropdown upgradesDropDown;
    [SerializeField] private InventoryUpgradeDisplayManager inventoryUpgradeDisplayManager;

    [SerializeField] private string upgradeLabel = "<Upgrade Storage>";
    [SerializeField] private int initMessengerBagCost = 10;
    [SerializeField] private int costPerMessengerBagLevel = 10;
    [SerializeField] private int frontBasketCost = 10;
    [SerializeField] private int rearBasketCost = 10;
    [SerializeField] private int saddlebagCost = 10;
    
    [SerializeField] private int cost = -1;
    [SerializeField] private UpgradeType upgradeType = UpgradeType.None;

    private string TypeToLabel(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.ExpandMessengerBag:
                return "Expand Messenger Bag";
            case UpgradeType.AddFrontBasket:
                return "Add Front Basket";
            case UpgradeType.AddRearBasket:
                return "Add Rear Basket";
            case UpgradeType.AddSaddlebags:
                return "Add Saddlebags";
        }
        return null;
    }

    private UpgradeType LabelToType(string label)
    {
        for (var type = UpgradeType.None; type <= UpgradeType.NumUpgradeTypes; ++type)
        {
            if (label == TypeToLabel(type)) return type;
        }
        return UpgradeType.None;
    }

    private void Start()
    {
        if (upgradeCostText == null)
        {
            Debug.LogError("upgradeCostText not set");
        }
        if (upgradesDropDown == null)
        {
            Debug.LogError("upgradesDropDown not set");
        }

        UpdateUpgrade(true);
    }

    public void UpdateUpgrade(bool updateDropDown)
    {
        Debug.Log("Updating Upgrade - Current: " + upgradeType.ToString() + " to: " + upgradesDropDown.captionText.text);
        
        // Update the drop down list
        if (updateDropDown)
        {
            int currentSelection = Math.Max(0, upgradesDropDown.value);

            List<string> options = new List<string>();
            options.Add(upgradeLabel);

            if (GameManager.instance.messengerBagLevel < (GameManager.instance.numMessengerBagLevels - 1))
            {
                options.Add(TypeToLabel(UpgradeType.ExpandMessengerBag));
            }
            if (!GameManager.instance.hasFrontBasket)
            {
                options.Add(TypeToLabel(UpgradeType.AddFrontBasket));
            }
            if (!GameManager.instance.hasRearBasket)
            {
                options.Add(TypeToLabel(UpgradeType.AddRearBasket));
            }
            if (!GameManager.instance.hasSaddlebags)
            {
                options.Add(TypeToLabel(UpgradeType.AddSaddlebags));
            }

            upgradesDropDown.ClearOptions();
            if (options.Count > 0) upgradesDropDown.AddOptions(options);

            upgradesDropDown.value = Math.Min(currentSelection, upgradesDropDown.options.Count - 1);
        }

        // Determine the current upgrade selection
        upgradeType = LabelToType(upgradesDropDown.captionText.text);

        // Determine the current upgrade cost
        switch (upgradeType)
        {
            case UpgradeType.ExpandMessengerBag:
                cost = initMessengerBagCost + GameManager.instance.numMessengerBagLevels * costPerMessengerBagLevel;
                break;
            case UpgradeType.AddFrontBasket:
                cost = frontBasketCost;
                break;
            case UpgradeType.AddRearBasket:
                cost = rearBasketCost;
                break;
            case UpgradeType.AddSaddlebags:
                cost = saddlebagCost;
                break;
            default:
                cost = -1;
                break;
        }
        if (cost >= 0)
        {
            upgradeCostText.text = cost.ToString();
        }
        else
        {
            upgradeCostText.text = "";
        }

        inventoryUpgradeDisplayManager.UpdateDisplay();
        inventoryUpgradeDisplayManager.SetDimmed(upgradeTypeToContainer[upgradeType]);
    }

    public void BuyUpgrade()
    {
        Debug.Log("Purchasing Upgrade: " + upgradeType.ToString() + " at Cost: " + cost + " Current funds: " + GameManager.instance.GetMoney());
        if (cost < 0)
        {
            return;
        }

        if (!GameManager.instance.SpendMoney(cost))
        {
            Debug.Log("Failed to buy upgrade - Cost: " + cost + " Available: " + GameManager.instance.GetMoney());
            return;
        }

        // Do the upgrade
        switch (upgradeType)
        {
            case UpgradeType.ExpandMessengerBag:
                ++GameManager.instance.messengerBagLevel;
                break;
            case UpgradeType.AddFrontBasket:
                GameManager.instance.hasFrontBasket = true;
                break;
            case UpgradeType.AddRearBasket:
                GameManager.instance.hasRearBasket = true;
                break;
            case UpgradeType.AddSaddlebags:
                GameManager.instance.hasSaddlebags = true;
                break;
            default:
                Debug.Log("Unrecognized upgrade type: " + upgradeType);
                return;
        }
        
        // Update the upgrade options and current selection
        UpdateUpgrade(true);
    }
}
