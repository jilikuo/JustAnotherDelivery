using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Required fields")]
    [SerializeField] private Image highlight;
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

    public void SetHighlight(Color color)
    {
        highlight.color = color;
        highlight.enabled = true;

        //Debug.Log("Set Highlight of: " + gameObject.name);
    }

    public void ClearHighlight()
    {
        highlight.enabled = false;
        highlight.color = Color.white;

        //Debug.Log("Cleared Highlight of: " + gameObject.name);
    }
}
