using DragDrop;
using JetBrains.Annotations;
using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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

    public void AddItem(string iconName, string fullName, string address, StorylineID storylineID, int cost)
    {
        packages.Add(new Package(iconName, fullName, address, storylineID, cost));
        Debug.Log("Added item to Inventory: " + fullName + "@" + address);
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
public class Address
{
    public string fullName;
    public string address;

    public Address(string fullName, string address)
    {
        this.fullName = fullName;
        this.address = address;
    }

    public Address(Characters character)
    {
        this.fullName = character.fullName;
        this.address = character.waypoint.GetFullAddress();
    }

    // Helper for Load
    public Address(string attributes)
    {
        var parsed = attributes.Split('@');
        this.fullName = parsed[0];
        this.address = parsed[1];
    }

    // Helper for Save
    public override string ToString()
    {
        return string.Join("@", fullName, address);
    }

    public Address Clone()
    {
        return new Address(fullName, address);
    }
}

[Serializable]
public class Package
{
    public string iconName;
    public Address address;
    public StorylineID storylineID;
    public int cost;


    public Package(string iconName, Address address, StorylineID storylineID, int cost)
    {
        this.iconName = iconName;
        this.address = address;
        this.storylineID = storylineID;
        this.cost = cost;
    }

    public Package(string iconName, string fullName, string address, StorylineID storylineID, int cost)
    {
        this.iconName = iconName;
        this.address = new Address(fullName, address);
        this.storylineID = storylineID;
        this.cost = cost;
    }

    // Helper for Load
    public Package(string attributes)
    {
        var parsed = attributes.Split(':');
        this.iconName = parsed[0];
        this.address = new Address(parsed[1]);
        this.storylineID = (StorylineID)Convert.ToInt32(parsed[2]);
        this.cost = Convert.ToInt32(parsed[3]);
    }

    // Helper for Save
    public override string ToString()
    {
        return string.Join(":", iconName, address, (int)storylineID, cost);
    }

    public string ToDisplayString()
    {
        return address.ToString() + ", Coins: " + cost;
    }
}
