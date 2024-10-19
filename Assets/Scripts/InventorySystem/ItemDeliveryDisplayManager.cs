using DragDrop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeliveryDisplayManager : MonoBehaviour, IItemDroppable
{
    public GameObject deliveryBox;
    private Inventory inventory; 
    private NavigationMenuHandler navigationManager;

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

        navigationManager = GameObject.FindGameObjectWithTag("NavigationManager").GetComponent<NavigationMenuHandler>();
        if (navigationManager == null)
        {
            Debug.LogError("Failed to locate NavigationMenuHandler");
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

        deliveryBox.SetActive(false);
        navigationManager.SetMessage("Thank you for the delivery!");
    }

    public bool IsValidDropPosition(DragDropObject item)
    {
        Characters activeNpc = navigationManager.GetActiveNPC();
        GameObject itemGameObject = item.gameObject;
        DragDropPackage package = itemGameObject.GetComponent<DragDropPackage>();

        if (package.data.address.fullName != activeNpc.fullName)
        {
            navigationManager.SetMessage("This is not mine!");
            return false;
        }

        return true;
    }
}
