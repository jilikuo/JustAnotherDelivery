using DragDrop;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class InventoryDisplayManager : MonoBehaviour, IItemDraggable, IItemDroppable
{
    [Header("Required fields")]
    [SerializeField] private TextMeshProUGUI inventoryLabel;
    [SerializeField] private GameObject inventoryBackground;
    [Header("Optional fields")]
    [Header("Inventory defaults to PlayerInventory")]
    [SerializeField] private Inventory inventory;
    [Header("Inventory Config defaults to Inventory.inventoryConfig")]
    [SerializeField] private InventoryConfigObject inventoryConfig;
    [Header("Derived fields")]
    [SerializeField] private GameObject inventoryLayout;
    [SerializeField] private GameObject gridObject;
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    // Start is called before the first frame update
    private void Start()
    {
        if (inventory == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Failed to locate Player");
            }
            inventory = player.inventory;
            if (inventory == null)
            {
                Debug.LogError("Failed to locate inventory");
            }
        }
        if (inventoryConfig == null)
        {
            inventoryConfig = inventory.inventoryConfig;
            if (inventoryConfig == null)
            {
                Debug.LogError("Failed to locate inventoryConfig");
            }
        }
        SetInventoryConfig(inventoryConfig);
    }

    private bool HasLayout()
    {
        return inventoryLayout != null;
    }

    public void SetInventoryConfig(InventoryConfigObject newConfig)
    {
        if (inventoryLayout != null)
        {
            Destroy(inventoryLayout.gameObject);
            inventoryLayout = null;
        }
        if (newConfig != null)
        {
            inventoryConfig = newConfig;

            inventoryLayout = Instantiate(newConfig.layoutPrefab, inventoryBackground.transform);
            var rect = inventoryLayout.GetComponent<RectTransform>();
            var anchorPoint = new Vector2(0.5f, 0.5f);
            rect.anchorMin = anchorPoint;
            rect.anchorMax = anchorPoint;
            rect.pivot = anchorPoint;
            inventoryLayout.transform.position = inventoryBackground.transform.position;

            // Update the inventory label
            inventoryLabel.text = inventoryConfig.label;

            // Locate the grid
            gridObject = inventoryLayout.transform.gameObject.transform.Find("Grid").gameObject;

            // Locate the inventorySlots
            inventorySlots = new List<InventorySlot>();
            for (int i = 0; i < gridObject.transform.childCount; i++)
            {
                var child = gridObject.transform.GetChild(i);
                var slot = child.GetComponent<InventorySlot>();
                if (slot == null)
                {
                    slot = child.AddComponent<InventorySlot>();
                }
                inventorySlots.Add(slot);
            }
        }
        else
        {
            inventoryLabel.text = "";
            gridObject = null;
            inventorySlots = null;
        }
    }

    public void RemoveDragDropObject(DragDropObject item)
    {
        if (!HasLayout())
        {
            return;
        }

        // Clear all references to the item
        foreach (var slot in inventorySlots)
        {
            if (slot.item == item)
            {
                slot.item = null;
            }
        }

        // Remove the item from the grid
        if (item.gameObject.transform.parent == gridObject.transform)
        {
            item.gameObject.transform.parent = null;
        }

        // Remove the item from the inventory
        inventory.RemoveItem(item.gameObject);
    }

    public bool IsValidDropPosition(DragDropObject item)
    {
        if (!HasLayout())
        {
            return false;
        }

        if (item == null)
        {
            return false;
        }

        // Find the bounds of all intersecting slots
        Bounds itemBounds = item.GetWorldBounds();
        int i = 0;
        for (; i < inventorySlots.Count(); ++i)
        {
            if (!inventorySlots[i].Intersects(itemBounds))
            {
                continue;
            }
            if (!inventorySlots[i].MaySet(item))
            {
                return false;
            }
            break;
        }
        if (i == inventorySlots.Count())
        {
            return false;
        }
        Bounds bounds = inventorySlots[i].bounds;
        for (; i < inventorySlots.Count(); ++i)
        {
            if (!inventorySlots[i].Intersects(itemBounds))
            {
                continue;
            }
            if (!inventorySlots[i].MaySet(item))
            {
                return false;
            }
            bounds.Encapsulate(inventorySlots[i].bounds);
        }

        return bounds.ContainsBounds(itemBounds);
    }

    public void AddDragDropObject(DragDropObject item)
    {
        if (!HasLayout())
        {
            return;
        }

        // Add references to the item in all intersecting slots, and
        // center the item within the intersected-slot bounds
        Bounds itemBounds = item.GetWorldBounds();
        int i = 0;
        for (; i < inventorySlots.Count(); ++i)
        {
            if (!inventorySlots[i].Intersects(itemBounds))
            {
                continue;
            }
            break;
        }
        if (i == inventorySlots.Count())
        {
            return;
        }
        Bounds bounds = inventorySlots[i].bounds;
        for (; i < inventorySlots.Count(); ++i)
        {
            if (!inventorySlots[i].Intersects(itemBounds))
            {
                continue;
            }
            bounds.Encapsulate(inventorySlots[i].bounds);
            inventorySlots[i].item = item;
        }

        item.transform.position = bounds.center;

        // Add the item to the grid
        item.gameObject.transform.SetParent(gridObject.transform);

        // Add the item to the inventory
        inventory.AddItem(item.gameObject);
    }
}