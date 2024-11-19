using DragDrop;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySortingDisplayManager : MonoBehaviour
{
    [Header("Required fields")]
    [SerializeField] private TextMeshProUGUI inventoryLabel;
    [SerializeField] private GameObject inventoryBackground;
    [Header("Derived fields")]
    [SerializeField] private InventoryConfigObject inventoryConfig;
    [SerializeField] private GameObject inventoryLayout;

    // Start is called before the first frame update
    private void Start()
    {
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
        if (inventoryLayout.GetComponentInChildren<InventorySortingContainerManager>() == null)
        {
            inventoryLayout.AddComponent<InventorySortingContainerManager>();
        }

        // Update the inventory label
        inventoryLabel.text = inventoryConfig.label;

        // Put the inventory label on top
        inventoryLabel.transform.SetAsLastSibling();
    }
}
