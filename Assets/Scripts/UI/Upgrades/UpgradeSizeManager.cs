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
    [SerializeField] private List<string> upgradeValues = new List<string> { "Small Messenger Bag", "Medium Messenger Bag", "Large Messenger Bag" };

    private void Start()
    {
        if (GameManager.instance.numMessengerBagLevels != upgradeValues.Count)
        {
            Debug.LogError("numMessengerBagLevels does not equal upgradeValues count");
        }
        SetVars();
        for (int i = 0; i < GameManager.instance.messengerBagLevel; ++i) initCost *= costPerLevel;
    }
    protected override string GetUpgradeLabel()
    {
        return upgradeLabel;
    }

    protected override void DoUpgrade()
    {
        if ((nextValue >= 0) && (nextValue < GameManager.instance.numMessengerBagLevels))
        {
            GameManager.instance.messengerBagLevel = nextValue;
            initCost *= costPerLevel;
        }
        else
        {
            initCost = -1;
        }
    }
    protected override void UpdateValues()
    {
        value = GameManager.instance.messengerBagLevel;
        nextValue = value + 1;
    }
	
    protected override string GetCurrentValue()
    {
        return upgradeValues[value];
    }

    protected override string GetNextValue()
    {
        return (nextValue < GameManager.instance.numMessengerBagLevels) ? upgradeValues[nextValue] : null;
    }

    protected override int GetUpgradeCost()
    {
        return initCost;
    }

    protected override bool HasUpdatedValue()
    {
        return value != GameManager.instance.messengerBagLevel;
    }
}
