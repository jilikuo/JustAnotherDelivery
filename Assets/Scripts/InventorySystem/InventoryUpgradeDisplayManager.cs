using DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class InventoryUpgradeDisplayManager : InventoryConfigDisplayManager
{   public enum InventoryContainer
    {
        None,
        MessengerBag,
        NextMessengerBag,
        FrontBasket,
        RearBasket,
        SaddleBags
    };

    [SerializeField] protected float dimScale = .42f;

    protected Dictionary<GameObject, Color> backgroundColors = new Dictionary<GameObject, Color>();
    protected Dictionary<GameObject, Color> dimmedColors = new Dictionary<GameObject, Color>();
    protected List<GameObject> dimmed = new List<GameObject>();

    protected override void AfterStart()
    {
        Action<GameObject> AddContainerVars = go =>
        {
            Color color = go.GetComponent<Image>().color;
            Color dimmedColor = color;
            dimmedColor.a *= dimScale;
            backgroundColors.Add(go, color);
            dimmedColors.Add(go, dimmedColor);
        };

        foreach (var container in containers)
        {
            AddContainerVars(container);
        }
    }

    public void SetDimmed(InventoryContainer container)
    {
        var lastDimmed = dimmed;
        dimmed = new List<GameObject>();
        switch (container)
        {
            case InventoryContainer.None:
                break;
            case InventoryContainer.MessengerBag:
                dimmed.Add(messengerBags[GameManager.instance.messengerBagLevel]);
                break;
            case InventoryContainer.NextMessengerBag:
                dimmed.Add(messengerBags[GameManager.instance.messengerBagLevel + 1]);
                break;
            case InventoryContainer.FrontBasket:
                dimmed.Add(frontBasket);
                break;
            case InventoryContainer.RearBasket:
                dimmed.Add(rearBasket);
                break;
            case InventoryContainer.SaddleBags:
                dimmed.Add(leftSaddleBag);
                dimmed.Add(rightSaddleBag);
                break;
            default:
                Debug.LogError("Unrecognized InventoryContainer: " + container);
                break;
        }

        foreach (var go in lastDimmed)
        {
            if (!dimmed.Contains(go))
            {
                go.GetComponent<Image>().color = backgroundColors[go];
            }
        }

        foreach (var go in dimmed)
        {
            go.GetComponent<Image>().color = dimmedColors[go];
        }
    }

    public override void UpdateDisplay()
    {
        base.UpdateDisplay();

        foreach (GameObject go in dimmed)
        {
            go.SetActive(true);
        }
    }
}
