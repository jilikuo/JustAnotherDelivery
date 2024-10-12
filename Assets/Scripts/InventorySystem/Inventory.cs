using DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [Header("Next Inventory Configuration")]
    public InventoryConfigObject inventoryConfig;
    public RandomStringGenerator packageAddressGen;
    public RandomGameObjectGenerator packageIconGen;

    [Header("Current Inventory")]
    public List<Package> packages = new List<Package>();

    public void AddItem(GameObject icon, string address)
    {
        packages.Add(new Package(icon, address));
        Debug.Log("Added item to Inventory: " + address);
    }

    public void Reset()
    {
        packages = new List<Package>();
    }

    public void AddItem(GameObject item)
    {
        if (item == null)
        {
            return;
        }

        var package = item.GetComponent<DragDropPackage>();
        if (package == null)
        {
            Debug.LogError("Failed to add item to Inventory - Failed to locate Package component");
            return;
        }
        if (package.data == null)
        {
            Debug.LogError("Failed to add item to Inventory - Failed to locate Package data");
            return;
        }
        packages.Add(package.data);
        Debug.Log("Added item to Inventory: " + package.data.address);
    }

    public void RemoveItem(GameObject item)
    {
        if (item == null)
        {
            return;
        }

        var package = item.GetComponent<DragDropPackage>();
        if (package == null)
        {
            Debug.LogError("Failed to remove item from Inventory - Failed to locate Package component");
            return;
        }
        if (package.data == null)
        {
            Debug.LogError("Failed to remove item from  Inventory - Failed to locate Package data");
            return;
        }
        packages.Remove(package.data);
        Debug.Log("Removed item from Inventory: " + package.data.address);
    }
}

[Serializable]
public class Package
{
    public GameObject icon;
    public string address;

    public Package(GameObject icon, string address)
    {
        this.icon = icon;
        this.address = address;
    }
}
