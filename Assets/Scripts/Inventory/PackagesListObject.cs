using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory Config", menuName = "Inventory System/Packages List")]
public class PackagesListObject : ScriptableObject
{
    public List<string> addresses;
    public List<ItemObject> packageItems;
}
