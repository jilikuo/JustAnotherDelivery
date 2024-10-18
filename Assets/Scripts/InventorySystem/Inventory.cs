using DragDrop;
using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, ISaveable
{
    [Header("Current Inventory")]
    public List<Package> packages = new List<Package>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Inventory");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Save(GameData gameData)
    {
        gameData.inventoryPackagesData = new List<string>();
        var data = gameData.inventoryPackagesData;
        foreach (var package in packages)
        {
            data.Add(package.ToString());
        }
    }

    public bool Load(GameData gameData)
    {
        packages = new List<Package>();
        foreach (var val in gameData.inventoryPackagesData)
        {
            packages.Add(new Package(val));
        }

        return true;
    }

    public void AddItem(string iconName, string address, int cost)
    {
        packages.Add(new Package(iconName, address, cost));
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
    public string iconName;
    public string address;
    public int cost;

    public Package(string iconName, string address, int cost)
    {
        this.iconName = iconName;
        this.address = address;
        this.cost = cost;
    }

    // Helper for Load
    public Package(string attributes)
    {
        var parsed = attributes.Split(':');
        this.iconName = parsed[0];
        this.address = parsed[1];
        this.cost = Convert.ToInt32(parsed[2]);
    }

    // Helper for Save
    public override string ToString()
    {
        return string.Join(":", iconName, address, cost);
    }
}
