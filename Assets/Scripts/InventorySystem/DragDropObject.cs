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
    protected Vector2 dragOffset = Vector2.zero;
    protected Vector3 startPosition = Vector3.zero;
    protected Quaternion startRotation = Quaternion.identity;
    protected IItemDraggable itemDraggableParent;
    protected Transform oldParent;
    protected RectTransform rectTransform;
    protected int siblingIndex = -1;

    public Bounds GetWorldBounds()
    {
        return gameObject.GetWorldBounds();
    }

    protected virtual void BeforeBeginDrag(PointerEventData eventData)
    {
    }

    protected virtual void AfterBeginDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!eventData.button.Equals(PointerEventData.InputButton.Left)) return;
        //Debug.Log("OnBeginDrag: From: " + transform.position + " To: " + eventData.position);
        BeforeBeginDrag(eventData);

        if (currentDragDropObject != null)
        {
            OnEndDrag(eventData);
        }
        currentDragDropObject = this;
        startPosition = transform.position;
        startRotation = transform.rotation;
        dragOffset = (Vector2)transform.position - eventData.position;

        rectTransform = transform as RectTransform;

        // Convert screen position to world position relative to the rectTransform
        Vector3 globalMousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos);

        // Calculate the world size of the object considering its scale
        Vector2 worldSizeObject = new Vector2(
            rectTransform.sizeDelta.x * rectTransform.lossyScale.x,
            rectTransform.sizeDelta.y * rectTransform.lossyScale.y
        );

        // Calculate the drag offset based on the pivot point
        dragOffset = new Vector2((0.5f - rectTransform.pivot.x) * worldSizeObject.x, (0.5f - rectTransform.pivot.y) * worldSizeObject.y);

        // Transfer the object to the root, to keep it always on top
        itemDraggableParent = GetComponentInParent<IItemDraggable>();
        oldParent = transform.parent;
        siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(transform.GetComponent<RectTransform>().root, true);
        transform.SetAsLastSibling();

        // Calculate the position of the object based on the global mouse position and the drag offset,
        // updating the object's position to follow the mouse while considering its pivot point.
        transform.position = globalMousePos - (Vector3)dragOffset;


        AfterBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.button.Equals(PointerEventData.InputButton.Left)) return;

        //Debug.Log("OnDrag: From: " + transform.position + " To: " + eventData.position);

        Vector3 globalMousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos);

        transform.position = globalMousePos - (Vector3)dragOffset;

    }

    protected virtual void BeforeEndDrag(PointerEventData eventData)
    {
    }

    protected virtual void AfterEndDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!eventData.button.Equals(PointerEventData.InputButton.Left)) return;


        //Debug.Log("OnEndDrag: From: " + transform.position + " To: " + eventData.position);
        BeforeEndDrag(eventData);

        var newParent = FindItemDroppableIntersection(eventData);
        var itemDroppableParent = (newParent != null) ? newParent.GetComponent<IItemDroppable>() : null;
        if (itemDroppableParent != null && itemDroppableParent.IsValidDropPosition(this))
        {
            Vector3 globalMousePos;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                transform as RectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos);

            transform.position = globalMousePos - (Vector3)dragOffset;
            if (itemDraggableParent != null)
            {
                itemDraggableParent.RemoveDragDropObject(this);
            }
            transform.SetParent(newParent.transform, true);
            transform.SetAsLastSibling();
            itemDroppableParent.AddDragDropObject(this);
        }
        else
        {
            transform.position = startPosition;
            transform.rotation = startRotation;

            // Return the object to the previous parent
            transform.SetParent(oldParent, true);
            transform.SetSiblingIndex(siblingIndex);
        }

        itemDraggableParent = null;
        oldParent = null;
        siblingIndex = -1;
        currentDragDropObject = null;

        AfterEndDrag(eventData);
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




