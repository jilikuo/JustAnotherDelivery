using DragDrop;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class NextItemDisplayManager : MonoBehaviour, IItemDraggable
{
    [Header("Required fields")]
    [SerializeField] private TextMeshProUGUI nextItemLabel;
    [SerializeField] private GameObject nextItemBackground;
    [SerializeField] private TextMeshProUGUI addressLabel;
    [SerializeField] private PackageGenerator packageGenerator;
    [Header("Derived fields")]
    [SerializeField] private DragDropObject nextItem;

    private void Start()
    {
        if (nextItemLabel == null)
        {
            Debug.LogError("nextItemLabel is not set");
        }
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
                addressLabel.text = nextItem.GetComponent<DragDropPackage>().data.address;
            }
            else
            {
                addressLabel.text = "";
                // TODO: Report no more items
            }
        }
    }

    public void Reset()
    {
        if (nextItem != null)
        {
            Destroy(nextItem.gameObject);
            nextItem = null;
        }
        packageGenerator.Reset();
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
