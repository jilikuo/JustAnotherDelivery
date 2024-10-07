using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour
{
    [Header("Required fields")]
    [SerializeField] private Image highlight;
    [SerializeField] private Color highlightGood = new Color(0f, 1f, 0f, .75f);
    [SerializeField] private Color highlightBad = new Color(1f, 0f, 0f, .75f);
    [Header("Optional fields")]
    public DragDropObject item;
    [Header("Derived fields")]
    public Bounds bounds;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    // Highlight this slot, if the current DragDrop item is above it
    private void Update()
    {
        HighlightOnIntersect(DragDropObject.currentDragDropObject);
    }

    public void Init()
    {
        if (highlight == null)
        {
            var images = transform.GetComponentsInChildren<Image>();
            if (images.Count() > 0)
            {
                highlight = images.Last();
            }
            else
            {
                Debug.LogError("Highlight image not found");
            }
        }
        bounds = highlight.GetWorldBounds();
        ClearHighlight();
    }

    public bool IsEmpty()
    {
        return this.item == null;
    }

    // Returns whether this slot may be set to the given item
    // * The item may be set, if this slot is empty, or if it already contains the item.
    public bool MaySet(DragDropObject item)
    {
        return IsEmpty() || this.item == item;
    }

    public bool Intersects(Bounds itemBounds)
    {
        if (itemBounds == null)
        {
            return false;
        }
        return itemBounds.Intersects(this.bounds);
    }

    public bool Intersects(DragDropObject item)
    {
        if (item == null)
        {
            return false;
        }
        return Intersects(item.GetWorldBounds());
    }

    public void ClearHighlight()
    {
        highlight.enabled = false;
        highlight.color = Color.white;

        //Debug.Log("Cleared Highlight of: " + gameObject.name);
    }

    // If the item does not intersect, clear highlighting in the slot.
    // Otherwise, highlight in highlightGood, if this slot can take the item.
    // * This slot can take the item, if this slot is empty, or if the item is already in this slot.
    // Otherwise, highlight in highlightBad.
    //
    // Returns: whether this slot may be set to the item
    public bool HighlightOnIntersect(DragDropObject item)
    {
        if (!Intersects(item))
        {
            ClearHighlight();
            return false;
        }
        bool canSet = MaySet(item);
        highlight.color = (canSet) ? highlightGood : highlightBad;
        highlight.enabled = true;

        //Debug.Log("Highlighted: " + gameObject.name);
        return canSet;
    }
}
