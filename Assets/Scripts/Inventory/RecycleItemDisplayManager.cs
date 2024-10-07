using DragDrop;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RecycleItemDisplayManager : MonoBehaviour, IItemDroppable
{
    [SerializeField] private TextMeshProUGUI recycleItemLabel;
    [SerializeField] private GameObject recycleItemBackground;
    [SerializeField] private PackageGenerator packageGenerator;

    private void Awake()
    {
        if (recycleItemLabel == null)
        {
            Debug.LogError("recycleItemLabel is not set");
        }
        if (recycleItemBackground == null)
        {
            Debug.LogError("recycleItemBackground is not set");
        }
        if (packageGenerator == null)
        {
            Debug.LogError("packageGenerator is not set");
        }
    }

    public void AddDragDropObject(DragDropObject item)
    {
        if (item  == null)
        {
            return;
        }
        // TODO: Add recycling effect
        item.transform.SetParent(null);
        packageGenerator.ReturnDragDrop(item);
    }

    public bool IsValidDropPosition(DragDropObject item)
    {
        return true;
    }
}
