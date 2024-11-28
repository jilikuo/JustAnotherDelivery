using DragDrop;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NextItemDisplayManager : MonoBehaviour, IItemDraggable
{
    [Header("Required fields")]
    [SerializeField] private GameObject nextItemBackground;
    [SerializeField] private TextMeshProUGUI addressLabel;
    [SerializeField] private InventorySortingPackageGenerator packageGenerator;
    [Header("Derived fields")]
    [SerializeField] private DragDropObject nextItem;

    private void Start()
    {
        if (nextItemBackground == null)
        {
            Debug.LogError("nextItemBackground is not set");
        }
        if (addressLabel == null)
        {
            Debug.LogError("addressLabel is not set");
        }
        if (packageGenerator == null)
        {
            Debug.LogError("packageGenerator is not set");
        }
    }

    private void Update()
    {
        if (nextItem == null)
        {
            nextItem = packageGenerator.CreateDragDrop(nextItemBackground);
            if (nextItem != null)
            {
                addressLabel.text = nextItem.GetComponent<InventorySortingPackage>().data.ToDisplayString();

                // Put the address label on top
                addressLabel.transform.SetAsLastSibling();
            }
            else
            {
                addressLabel.text = "";
                // TODO: Report no more items
            }
        }
    }

    public void RemoveDragDropObject(DragDropObject item)
    {
        if (item  == null)
        {
            return;
        }
        if (nextItem == item)
        {
            nextItem = null;
        }
        if (item.transform.parent == nextItemBackground.transform)
        {
            item.transform.SetParent(null);
        }
    }
}
