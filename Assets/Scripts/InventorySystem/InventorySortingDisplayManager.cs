using DragDrop;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySortingDisplayManager : MonoBehaviour, IItemDraggable, IItemDroppable
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
    [SerializeField] private List<InventorySortingPackage> packages = new List<InventorySortingPackage>();

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
    }

    private void LateUpdate()
    {
        HighlightIntersecting(DragDropObject.currentDragDropObject);
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
        // Remove the item from the display list
        packages.Remove(item as InventorySortingPackage);

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

    // Highlight good/bad, for available drop point
    // Return true, if the item may be dropped at its present location;
    // otherwise, return false.
    private bool HighlightIntersecting(DragDropObject item)
    {
        if (item == null)
        {
            ClearHighlight();
            return false;
        }

        var gridBounds = gridObject.GetWorldBounds();
        var itemBounds = item.GetWorldBounds();
        if (!gridBounds.Intersects(itemBounds))
        {
            ClearHighlight();
            return false;
        }

        if (!gridBounds.ContainsBounds(itemBounds))
        {
            SetHighlight(highlightBad);
            return false;
        }

        foreach (var package in packages)
        {
            if (package.IsColliding())
            {
                SetHighlight(highlightBad);
                return false;
            }
        }

        SetHighlight(highlightGood);
        return true;
    }

    public void AddDragDropObject(DragDropObject item)
    {
        // Add the item to the display list
        packages.Add(item as InventorySortingPackage);

        // Add the item to the grid
        item.gameObject.transform.SetParent(gridObject.transform);

        // Add the item to the inventory
        inventory.AddItem(item.gameObject);

        // Put the inventory label on top
        inventoryLabel.transform.SetAsLastSibling();
    }
}