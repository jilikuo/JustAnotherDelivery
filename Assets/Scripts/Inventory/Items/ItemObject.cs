using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items")]
public class ItemObject : ScriptableObject
{
    public GameObject icon;

    public string label = "Item Name";
    public string description = "Item Description";
}

public static class ScriptableObjectExtension
{
    public static T Clone<T> (this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null) return null;

        T instance = UnityEngine.Object.Instantiate (scriptableObject);
        instance.name = scriptableObject.name; // remove (Clone) from name
        return instance;
    }    
}
