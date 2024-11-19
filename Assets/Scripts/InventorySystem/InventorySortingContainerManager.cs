using DragDrop;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySortingContainerManager : MonoBehaviour, IItemDraggable, IItemDroppable
{
    [Header("Required fields")]
    [SerializeField] private GameObject container;
    [Header("Optional fields")]
    [Header("Inventory defaults to Inventory")]
    [SerializeField] private Inventory inventory;
    [Header("Grid defaults to Grid")]
    [SerializeField] private GameObject grid;
    [SerializeField] private Color highlightGood = new Color(0f, 1f, 0f, .40f);
    [SerializeField] private Color highlightBad = new Color(1f, 0f, 0f, .40f);
    [Header("Derived fields")]
    [SerializeField] private List<InventorySortingPackage> packages = new List<InventorySortingPackage>();

    private Image containerBackground;
    private Color backgroundColor;

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
        if (container == null)
        {
            container = gameObject;
        }
        containerBackground = container.GetComponent<Image>();
        if (grid == null)
        {
            grid = container.transform.gameObject.transform.Find("Grid").gameObject;
            if (inventory == null)
            {
                Debug.LogError("Failed to locate grid");
            }
        }
        backgroundColor = containerBackground.color;
    }

    private void LateUpdate()
    {
        HighlightIntersecting(DragDropObject.currentDragDropObject);
    }

    public void SetHighlight(Color color)
    {
        containerBackground.color = color;
    }

    public void ClearHighlight()
    {
        containerBackground.color = backgroundColor;
    }

    public void RemoveDragDropObject(DragDropObject item)
    {
        // Remove the item from the display list
        packages.Remove(item as InventorySortingPackage);

        // Remove the item from the grid
        if (item.gameObject.transform.parent == container.transform)
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

        var gridBounds = grid.GetWorldBounds();
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
        item.gameObject.transform.SetParent(grid.transform);

        // Add the item to the inventory
        inventory.AddItem(item.gameObject);
    }
}