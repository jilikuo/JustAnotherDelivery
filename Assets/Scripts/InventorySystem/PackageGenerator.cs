using DragDrop;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PackageGenerator : MonoBehaviour, IDragDropGenerator
{
    [Header("Required Fields")]
    [SerializeField] private RandomAddressGenerator packageAddressGen;
    [SerializeField] private float imageScale = 75;
    [Header("Derived Fields")]
    [SerializeField] private RandomGameObjectGenerator packageIconGen;

    private Inventory inventory;

    private void Start()
    {
        var inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Failed to locate Inventory");
        }
        packageIconGen = GameManager.instance.packageIconGen;
        if (packageIconGen == null)
        {
            Debug.LogError("Failed to locate packageIconGen");
        }
        if (packageAddressGen == null)
        {
            Debug.LogError("Failed to locate packageAddressGen");
        }
        packageAddressGen.Clear();
        foreach (var character in GameManager.instance.npcCollection.NPCList)
        {
            packageAddressGen.AddEntry(new Address(character));
        }
    }

    private int CalcCost(GameObject icon)
    {
        // Arbitrary cost value, currently based on icon size
        var rect = icon.GetComponent<RectTransform>();
        float cost = Mathf.Max(1, rect.sizeDelta.x * rect.sizeDelta.y);
        return (int)(GameManager.instance.packageValueMultiplier * cost);
    }

    public DragDropObject CreateDragDrop(GameObject parent)
    {
        Address address = packageAddressGen.GetEntry();
        if (address == null)
        {
            return null;
        }
        packageAddressGen.RemoveEntry(address);
        GameObject packageIcon = packageIconGen.GetEntry();

        var icon = Instantiate(packageIcon, parent.transform);
        var rect = icon.GetComponent<RectTransform>();
        rect.localScale = new Vector3(imageScale, imageScale, 1f);
        var centerPoint = new Vector2(0.5f, 0.5f);
        rect.pivot = centerPoint;
        rect.anchorMin = centerPoint; // Bottom-left corner
        rect.anchorMax = centerPoint; // Top-right corner      
        icon.transform.position = parent.transform.position;

        var dragDrop = icon.AddComponent<DragDropPackage>();
        dragDrop.data = new Package(packageIcon.name, address, CalcCost(icon));

        return dragDrop;
    }

    public void ReturnDragDrop(DragDropObject item)
    {
        var package = item.GetComponent<DragDropPackage>();
        if (package == null)
        {
            Debug.LogError("Failed to locate Package component");
            return;
        }
        if (package.data == null)
        {
            Debug.LogError("Failed to locate Package data");
            return;
        }
        packageAddressGen.AddEntry(package.data.address);
        Destroy(item.gameObject);
    }
}
