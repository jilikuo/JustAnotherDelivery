using DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
        List<GameObject> gos = new List<GameObject>();
        switch (container)
        {
            case InventoryContainer.None:
                break;
            case InventoryContainer.MessengerBag:
                gos.Add(messengerBags[GameManager.instance.messengerBagLevel]);
                break;
            case InventoryContainer.NextMessengerBag:
                gos.Add(messengerBags[GameManager.instance.messengerBagLevel + 1]);
                break;
            case InventoryContainer.FrontBasket:
                gos.Add(frontBasket);
                break;
            case InventoryContainer.RearBasket:
                gos.Add(rearBasket);
                break;
            case InventoryContainer.SaddleBags:
                gos.Add(leftSaddleBag);
                gos.Add(rightSaddleBag);
                break;
            default:
                Debug.LogError("Unrecognized InventoryContainer: " + container);
                break;
        }

        foreach (GameObject go in containers)
        {
            if (gos.Contains(go))
            {
                go.GetComponent<Image>().color = dimmedColors[go];
                go.SetActive(true);
            }
            else
            {
                go.GetComponent<Image>().color = backgroundColors[go];
            }
        }
    }
}
