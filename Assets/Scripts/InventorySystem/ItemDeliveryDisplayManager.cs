using DragDrop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeliveryDisplayManager : MonoBehaviour, IItemDroppable
{
    public GameObject deliveryBox;
    private Inventory inventory;

    void Start() 
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        if (deliveryBox == null)
        {
            Debug.LogError("deliveryBox is not set");
        }
        if (inventory == null)
        {
            Debug.LogError("inventory is not set");
        }
    }

    public void AddDragDropObject(DragDropObject item)
    {
        GameObject itemGameObject = item.gameObject;
        DragDropPackage package = itemGameObject.GetComponent<DragDropPackage>();

        if (item == null || itemGameObject == null)
        {
            return;
        }

        GameManager.instance.RewardForDelivery(package.data);
        Destroy(itemGameObject);
    }

    public bool IsValidDropPosition(DragDropObject item)
    {
        //TODO: If active character != recipient, return false
        return true;
    }
}
