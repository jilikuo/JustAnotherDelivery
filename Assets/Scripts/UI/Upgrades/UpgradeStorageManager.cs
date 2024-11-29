using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStorageManager : UpgradePanelManagerBase
{   enum UpgradeType
    {
        None,
        ExpandMessengerBag,
        AddFrontBasket,
        AddRearBasket,
        AddSaddlebags,
        NumUpgradeTypes
    };
    static Dictionary<UpgradeType, InventoryConfigDisplayManager.InventoryContainer> upgradeTypeToContainer =
    new Dictionary<UpgradeType, InventoryConfigDisplayManager.InventoryContainer>() {
            {UpgradeType.None, InventoryConfigDisplayManager.InventoryContainer.None},
            {UpgradeType.ExpandMessengerBag, InventoryConfigDisplayManager.InventoryContainer.NextMessengerBag},
            {UpgradeType.AddFrontBasket, InventoryConfigDisplayManager.InventoryContainer.FrontBasket},
            {UpgradeType.AddRearBasket, InventoryConfigDisplayManager.InventoryContainer.RearBasket},
            {UpgradeType.AddSaddlebags,InventoryConfigDisplayManager.InventoryContainer.SaddleBags }
        };

    [SerializeField] private string upgradeLabel = "Storage Upgrades";
    [SerializeField] private TMPro.TMP_Dropdown upgradesDropDown;
    [SerializeField] private InventoryConfigDisplayManager inventoryConfigDisplayManager;

    [SerializeField] private int initMessengerBagCost = 10;
    [SerializeField] private int costPerMessengerBagLevel = 10;
    [SerializeField] private int frontBasketCost = 10;
    [SerializeField] private int rearBasketCost = 10;
    [SerializeField] private int saddlebagCost = 10;
    
    [SerializeField] private string upgradeText = "";
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
        if (upgradesDropDown == null)
        {
            Debug.LogError("upgradesDropDown not set");
        }
        SetVars();
        UpdateSelections();
    }

    private void UpdateSelections()
    {
        int currentSelection = Math.Max(0, upgradesDropDown.value);

        List<string> options = new List<string>();

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
        if (options.Count > 0)
        {
            upgradesDropDown.AddOptions(options);
            upgradesDropDown.value = Math.Min(currentSelection, upgradesDropDown.options.Count - 1);
        }
    }

    protected override void EnableUpgradePanel(bool enable)
    {
        base.EnableUpgradePanel(enable);

        upgradesDropDown.enabled = enabled;
    }

    protected override string GetUpgradeLabel()
    {
        return upgradeLabel;
    }

    protected override int GetUpgradeCost()
    {
        switch (upgradeType)
        {
            case UpgradeType.ExpandMessengerBag:
                return initMessengerBagCost + GameManager.instance.numMessengerBagLevels * costPerMessengerBagLevel;
            case UpgradeType.AddFrontBasket:
                return frontBasketCost;
            case UpgradeType.AddRearBasket:
                return rearBasketCost;
            case UpgradeType.AddSaddlebags:
                return saddlebagCost;
            default:
                return -1;
        }
    }

    protected override bool HasUpdatedValue()
    {
        return upgradeText != upgradesDropDown.captionText.text;
    }

    protected override void UpdateValues()
    {
        UpdateSelections();

        upgradeText = upgradesDropDown.captionText.text;
        upgradeType = LabelToType(upgradeText);
    }

    protected override void UpdateDisplay()
    {
        inventoryConfigDisplayManager.SetDimmed(upgradeTypeToContainer[upgradeType]);
        inventoryConfigDisplayManager.UpdateDisplay();
    }

    protected override void DoUpgrade()
    {
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
    }
}
