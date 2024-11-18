using DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySortingPackage : DragDropPackage
{
    private Image icon;
    private Color backgroundColor;
    private Color highlightColor = new Color(1f, 0f, 0f, .75f);

    private bool isColliding = false;
    private Collider2D coll;

    private void Start()
    {
        icon = gameObject.GetComponent<Image>();
        backgroundColor = icon.color;

        coll = GetComponent<Collider2D>();
        if (coll == null)
        {
            Debug.Log("Created Collider2D of: " + gameObject.name);
            coll = gameObject.AddComponent<PolygonCollider2D>();
        }

        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.Log("Created RigidBody2D of: " + gameObject.name);
            rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        }
        rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2D.gravityScale = 0f;
    }

    protected override void BeforeBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Enable Collider Trigger of: " + gameObject.name);

        coll.isTrigger = true;
        base.BeforeBeginDrag(eventData);
    }

    protected override void AfterEndDrag(PointerEventData eventData)
    {
        Debug.Log("Disable Collider Trigger of: " + gameObject.name);

        base.AfterEndDrag(eventData);
        coll.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Detected Trigger Enter of: " + gameObject.name);

        SetColliding();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Detected Trigger Exit of: " + gameObject.name);

        ClearColliding();
    }

    public bool IsColliding()
    {
        return isColliding;
    }

    public void SetColliding()
    {
        Debug.Log("Set Colliding of: " + gameObject.name);

        isColliding = true;
        icon.color = highlightColor;
    }

    public void ClearColliding()
    {
        Debug.Log("Clear Colliding of: " + gameObject.name);

        isColliding = false;
        icon.color = backgroundColor;
    }
}




