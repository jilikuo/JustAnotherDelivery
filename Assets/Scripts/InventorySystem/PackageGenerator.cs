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
    [SerializeField] private RandomStringGenerator initPackageAddressGen;
    [SerializeField] private RandomGameObjectGenerator initPackageIconGen;
    [SerializeField] private RandomStringGenerator packageAddressGen;
    [SerializeField] private RandomGameObjectGenerator packageIconGen;
    [SerializeField] private float imageScale = 75;

    private void Start()
    {
        if (initPackageAddressGen == null)
        {
            var inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
            if (inventory == null)
            {
                Debug.LogError("Failed to locate Inventory");
            }
            initPackageAddressGen = inventory.packageAddressGen;
            if (initPackageAddressGen == null)
            {
                Debug.LogError("Failed to locate initPackageAddressGen");
            }
        }
        if (initPackageIconGen == null)
        {
            var inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
            if (inventory == null)
            {
                Debug.LogError("Failed to locate Inventory");
            }
            initPackageIconGen = inventory.packageIconGen;
            if (initPackageIconGen == null)
            {
                Debug.LogError("Failed to locate initPackageIconGen");
            }
        }
        Reset();
    }

    public void Reset()
    {
        packageAddressGen = initPackageAddressGen.Clone();
        packageIconGen = initPackageIconGen.Clone();
    }

    public DragDropObject CreateDragDrop(GameObject parent)
    {
        string address = packageAddressGen.GetEntry();
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
        dragDrop.data = new Package(packageIcon.name, address);

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
