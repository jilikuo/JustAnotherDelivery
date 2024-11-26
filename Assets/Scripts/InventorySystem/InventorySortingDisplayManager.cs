using DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySortingDisplayManager : InventoryConfigDisplayManager
{
    protected override void AfterStart()
    {
        Action<GameObject> AddContainerMananager = go =>
        {
            if (go.GetComponentInChildren<InventorySortingContainerManager>() == null)
            {
                go.AddComponent<InventorySortingContainerManager>();
            }
        };

        foreach (var container in containers)
        {
            AddContainerMananager(container);
        }
    }
}
