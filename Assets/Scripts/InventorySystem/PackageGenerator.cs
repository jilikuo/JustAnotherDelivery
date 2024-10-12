using DragDrop;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEditor.Progress;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PackageGenerator : MonoBehaviour, IDragDropGenerator
{
    [SerializeField] private PackagesListObject initPackagesList;
    [Header("Packages List defaults to Player.inventory.packagesList")]
    [SerializeField] private PackagesListObject packagesList;

    private void Start()
    {
        if (initPackagesList == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Failed to locate Player");
            }
            initPackagesList = player.inventory.packagesList;
            if (initPackagesList == null)
            {
                Debug.LogError("Failed to locate initPackagesList");
            }
        }
        Reset();
    }

    public void Reset()
    {
        packagesList = initPackagesList.Clone();
    }

    public DragDropObject CreateDragDrop(GameObject parent)
    {
        if (packagesList.addresses.Count == 0)
        {
            return null;
        }

        string address = packagesList.addresses.RandomElement();
        packagesList.addresses.Remove(address);
        ItemObject packageItem = packagesList.packageItems.RandomElement();

        var icon = Instantiate(packageItem.icon, parent.transform);
        var rect = icon.GetComponent<RectTransform>();
        var anchorPoint = new Vector2(0.5f, 0.5f);
        rect.anchorMin = anchorPoint;
        rect.anchorMax = anchorPoint;
        rect.pivot = anchorPoint;
        icon.transform.position = parent.transform.position;

        var dragDrop = icon.AddComponent<DragDropPackage>();
        dragDrop.data = new Package(packageItem, address);

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
        packagesList.addresses.Add(package.data.address);
        Destroy(item.gameObject);
    }
}
