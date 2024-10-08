using DragDrop;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class InventoryScrollingDisplayManager : MonoBehaviour, IItemDraggable
{
    [Header("Required fields")]
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private GameObject inventoryEntryPrefab;
    [Header("Optional fields")]
    [Header("Inventory defaults to PlayerInventory")]
    [SerializeField] private Inventory inventory;
    [Header("Derived fields")]
    [SerializeField] private List<GameObject> inventoryEntries = new List<GameObject>();

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

        inventoryEntries = new List<GameObject>();
        foreach (var package in inventory.packages)
        {
            var entry = Instantiate(inventoryEntryPrefab);
            entry.GetComponentInChildren<TextMeshProUGUI>().text = package.address;
            var image = entry.GetComponentInChildren<Image>();
            var icon = Instantiate(package.item.icon, image.transform);
            entry.transform.SetParent(scrollViewContent.transform, false);
            var rect = icon.GetComponent<RectTransform>();
            rect.sizeDelta = icon.transform.parent.GetComponent<RectTransform>().sizeDelta;

            var dragDrop = entry.AddComponent<DragDropPackage>();
            dragDrop.data = package;

            inventoryEntries.Add(entry);
        }
    }

    public bool IsValidDropPosition(DragDropObject item)
    {
        return true;
    }

    public void RemoveDragDropObject(DragDropObject item)
    {
        inventoryEntries.Remove(item.gameObject);
        inventory.RemoveItem(item.gameObject);
    }
}
