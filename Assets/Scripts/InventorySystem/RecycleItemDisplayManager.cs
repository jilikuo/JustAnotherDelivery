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
    [SerializeField] private Color highlightColor = new Color(0f, 1f, 0f, .40f);
    // TODO: Remove itemText, if it will not be used in final UI
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private GameObject itemBackground;
    [SerializeField] private PackageGenerator packageGenerator;

    private Image backgroundImage;
    private Color backgroundColor;

    private void Awake()
    {
        if (itemText == null)
        {
            Debug.LogError("itemText is not set");
        }
        if (itemBackground == null)
        {
            Debug.LogError("itemBackground is not set");
        }
        if (packageGenerator == null)
        {
            Debug.LogError("packageGenerator is not set");
        }
    }

    private void Start()
    {
        if (itemText != null)
        {
            itemText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        backgroundImage = itemBackground.GetComponent<Image>();
        backgroundColor = backgroundImage.color;
    }

    private void Update()
    {
        var item = DragDropObject.currentDragDropObject;
        if (item == null)
        {
            ClearHighlight();
            return;
        }
        if (!backgroundImage.GetWorldBounds().Intersects(item.GetWorldBounds()))
        {
            ClearHighlight();
            return;
        }
        SetHighlight();
    }

    public void SetHighlight()
    {
        backgroundImage.color = highlightColor;
    }


    public void ClearHighlight()
    {
        backgroundImage.color = backgroundColor;
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
