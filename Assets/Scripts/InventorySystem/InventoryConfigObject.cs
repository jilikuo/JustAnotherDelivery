using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory Config", menuName = "Inventory System/Inventory Config")]
public class InventoryConfigObject : ScriptableObject
{
    [Header("Layout Prefab must contain a child, \"Grid,\" containing only item slots")]
    public GameObject layoutPrefab;

    public string label = "Inventory Config";
}
