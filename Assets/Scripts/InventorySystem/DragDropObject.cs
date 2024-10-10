using DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static DragDropObject currentDragDropObject;

    [Header("Pre-drag state")]
    protected Vector3 startPosition = Vector3.zero;
    protected IItemDraggable itemDraggableParent;
    protected Transform oldParent;
    protected int siblingIndex = -1;

    public Bounds GetWorldBounds()
    {
        return gameObject.GetWorldBounds();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag: From: " + transform.position + " To: " + eventData.position);
        if (currentDragDropObject != null)
        {
            OnEndDrag(eventData);
        }
        currentDragDropObject = this;
        startPosition = transform.position;

        // Transfer the object to the root, to keep it always on top
        itemDraggableParent = GetComponentInParent<IItemDraggable>();
        oldParent = transform.parent;
        siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(transform.GetComponent<RectTransform>().root, true);
        transform.SetAsLastSibling();

        transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag: From: " + transform.position + " To: " + eventData.position);
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag: From: " + transform.position + " To: " + eventData.position);
        var newParent = FindItemDroppableIntersection(eventData);
        var ItemDroppableParent = (newParent != null) ? newParent.GetComponent<IItemDroppable>() : null;
        if (ItemDroppableParent != null && ItemDroppableParent.IsValidDropPosition(this))
        {
            if (itemDraggableParent != null)
            {
                itemDraggableParent.RemoveDragDropObject(this);
            }
            transform.SetParent(newParent.transform, true);
            transform.SetAsLastSibling();
            ItemDroppableParent.AddDragDropObject(this);
        }
        else
        {
            transform.position = startPosition;

            // Return the object to the previous parent
            transform.SetParent(oldParent, true);
            transform.SetSiblingIndex(siblingIndex);
        }

        itemDraggableParent = null;
        oldParent = null;
        siblingIndex = -1;
        currentDragDropObject = null;
    }

    protected GameObject FindItemDroppableIntersection(PointerEventData eventData)
    {
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        var hit = hits.FirstOrDefault(t => t.gameObject.GetComponent<IItemDroppable>() != null);
        return (hit.isValid) ? hit.gameObject : null;
    }
}

namespace DragDrop
{
    public class DragDropPackage : DragDropObject
    {
        public Package data;
    }

    public interface IItemDraggable
    {
        public void RemoveDragDropObject(DragDropObject item);
    }

    public interface IItemDroppable
    {
        public bool IsValidDropPosition(DragDropObject item);
        public void AddDragDropObject(DragDropObject item);

    }

    public interface IDragDropGenerator
    {
        public DragDropObject CreateDragDrop(GameObject parent);
        public void ReturnDragDrop(DragDropObject item);
    }
}




