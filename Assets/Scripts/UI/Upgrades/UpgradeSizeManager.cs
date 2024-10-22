using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSizeManager : UpgradePanelManagerTextBase
{
    [SerializeField] private string upgradeLabel = "Messenger Bag Size";
    [SerializeField] private int initCost = 10; 
    [SerializeField] private int costPerLevel = 10;
    [SerializeField] private int value = -1;
    [SerializeField] private int nextValue = -1;

    private void Start()
    {
        SetVars();
        for (int i = 0; i < GameManager.instance.inventoryConfigIndex; ++i) initCost *= costPerLevel;
    }
    protected override string GetUpgradeLabel()
    {
        return upgradeLabel;
    }

    protected override void DoUpgrade()
    {
        if ((nextValue >= 0) && (nextValue < GameManager.instance.inventoryConfigs.Count))
        {
            GameManager.instance.inventoryConfigIndex = nextValue;
            initCost *= costPerLevel;
        }
        else
        {
            initCost = -1;
        }
    }
    protected override void UpdateValues()
    {
        value = GameManager.instance.inventoryConfigIndex;
        nextValue = value + 1;
    }
	
    protected override string GetCurrentValue()
    {
        var inventoryConfig = GameManager.instance.GetInventoryConfig(value);
        return (inventoryConfig != null) ? inventoryConfig.label : null;
    }

    protected override string GetNextValue()
    {
        var inventoryConfig = GameManager.instance.GetInventoryConfig(nextValue);
        return (inventoryConfig != null) ? inventoryConfig.label : null;
    }

    protected override int GetUpgradeCost()
    {
        return initCost;
    }

    protected override bool HasUpdatedValue()
    {
        return value != GameManager.instance.inventoryConfigIndex;
    }
}
