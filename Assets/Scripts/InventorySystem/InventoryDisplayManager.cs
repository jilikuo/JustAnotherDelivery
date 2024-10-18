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
    [Header("Inventory defaults to Inventory")]
    [SerializeField] private Inventory inventory;
    [Header("Inventory Config defaults to Inventory.inventoryConfig")]
    [SerializeField] private Color highlightGood = new Color(0f, 1f, 0f, .40f);
    [SerializeField] private Color highlightBad = new Color(1f, 0f, 0f, .40f);
    [Header("Derived fields")]
    [SerializeField] private InventoryConfigObject inventoryConfig;
    [SerializeField] private GameObject inventoryLayout;
    [SerializeField] private GameObject gridObject;
    [SerializeField] private Image layoutBackground;
    [SerializeField] private Color layoutColor;
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    // Start is called before the first frame update
    private void Start()
    {
        if (inventory == null)
        {
            inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
            if (inventory == null)
            {
                Debug.LogError("Failed to locate inventory");
            }
        }
        inventoryConfig = GameManager.instance.inventoryConfigs[GameManager.instance.inventoryConfigIndex];
        if (inventoryConfig == null)
        {
            Debug.LogError("Failed to locate inventoryConfig");
        }
        inventoryLayout = Instantiate(inventoryConfig.layoutPrefab, inventoryBackground.transform);
        var rect = inventoryLayout.GetComponent<RectTransform>();
        var anchorPoint = new Vector2(0.5f, 0.5f);
        rect.anchorMin = anchorPoint;
        rect.anchorMax = anchorPoint;
        rect.pivot = anchorPoint;
        inventoryLayout.transform.position = inventoryBackground.transform.position;
        layoutBackground = inventoryLayout.GetComponent<Image>();
        layoutColor = layoutBackground.color;

        // Update the inventory label
        inventoryLabel.text = inventoryConfig.label;

        // Put the inventory label on top
        inventoryLabel.transform.SetAsLastSibling();

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

    private void Update()
    {
        HighlightIntersecting(DragDropObject.currentDragDropObject);
    }

    private bool IsInit()
    {
        return inventoryLayout != null;
    }

    public void SetHighlight(Color color)
    {
        layoutBackground.color = color;
    }

    public void ClearHighlight()
    {
        layoutBackground.color = layoutColor;
    }

    public void RemoveDragDropObject(DragDropObject item)
    {
        if (!IsInit())
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
        return HighlightIntersecting(item);
    }

    private bool HighlightIntersecting(DragDropObject item)
    {
        if (!IsInit())
        {
            return false;
        }

        if (item == null)
        {
            ClearHighlight();
            foreach (var slot in inventorySlots)
            {
                slot.ClearHighlight();
            }
            return false;
        }

        // Find the bounds of all intersecting slots
        Bounds itemBounds = item.GetWorldBounds();
        Bounds gridBounds = item.GetWorldBounds();
        bool foundFirst = false;
        bool isDroppable = false;
        foreach (var slot in inventorySlots)
        {
            if (!slot.Intersects(itemBounds))
            {
                slot.ClearHighlight();
                continue;
            }
            if (!foundFirst)
            {
                foundFirst = true;
                gridBounds = slot.bounds;
                isDroppable = slot.MaySet(item);
            }
            else
            {
                gridBounds.Encapsulate(slot.bounds);
            }
            if (slot.MaySet(item))
            {
                slot.SetHighlight(highlightGood);
            }
            else
            {
                slot.SetHighlight(highlightBad);
                isDroppable = false;
            }
        }

        if (foundFirst)
        {
            isDroppable &= gridBounds.ContainsBounds(itemBounds);

            if (isDroppable)
            {
                SetHighlight(highlightGood);
            }
            else
            {
                SetHighlight(highlightBad);
            }
        }
        else
        {
            ClearHighlight();
        }

        return isDroppable;
    }

    public void AddDragDropObject(DragDropObject item)
    {
        if (!IsInit())
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

        // Put the inventory label on top
        inventoryLabel.transform.SetAsLastSibling();
    }
}