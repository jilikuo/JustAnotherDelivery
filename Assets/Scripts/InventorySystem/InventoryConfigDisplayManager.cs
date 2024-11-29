using DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryConfigDisplayManager : MonoBehaviour
{
    public enum InventoryContainer
    {
        None,
        MessengerBag,
        NextMessengerBag,
        FrontBasket,
        RearBasket,
        SaddleBags
    };

    [Header("Required fields")]
    [SerializeField] private List<GameObject> messengerBags;
    [SerializeField] private GameObject frontBasket;
    [SerializeField] private GameObject rearBasket;
    [SerializeField] private GameObject leftSaddleBag;
    [SerializeField] private GameObject rightSaddleBag;
    [SerializeField] private bool isDragDrop = false;
    [SerializeField] private float dimScale = .42f;

    private Dictionary<GameObject, Color> backgroundColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Color> dimmedColors = new Dictionary<GameObject, Color>();
    private List<GameObject> dimmed = new List<GameObject>();

    private List<GameObject> containers = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        if (GameManager.instance.numMessengerBagLevels != messengerBags.Count)
        {
            Debug.LogError("numMessengerBagLevels(" + GameManager.instance.numMessengerBagLevels + ") does not equal messengerBags count(" + messengerBags.Count + ")");
        }

        for (int i = 0; i < messengerBags.Count; i++)
        {
            containers.Add(messengerBags[i]);
        }
        containers.Add(frontBasket);
        containers.Add(rearBasket);
        containers.Add(leftSaddleBag);
        containers.Add(rightSaddleBag);

        if (isDragDrop)
        {
            Action<GameObject> AddContainerMananager = go =>
            {
                if (go.GetComponentInChildren<InventorySortingContainerManager>() == null)
                {
                    go.AddComponent<InventorySortingContainerManager>();
                }
            };

            foreach (var container in containers)
            {
                AddContainerMananager(container);
            }
        }

        Action<GameObject> AddContainerVars = go =>
        {
            Color color = go.GetComponent<Image>().color;
            Color dimmedColor = color;
            dimmedColor.a *= dimScale;
            backgroundColors.Add(go, color);
            dimmedColors.Add(go, dimmedColor);
        };

        foreach (var container in containers)
        {
            AddContainerVars(container);
        }

        UpdateDisplay();
    }
    public void SetDimmed(InventoryContainer container)
    {
        var lastDimmed = dimmed;
        dimmed = new List<GameObject>();
        switch (container)
        {
            case InventoryContainer.None:
                break;
            case InventoryContainer.MessengerBag:
                dimmed.Add(messengerBags[GameManager.instance.messengerBagLevel]);
                break;
            case InventoryContainer.NextMessengerBag:
                dimmed.Add(messengerBags[GameManager.instance.messengerBagLevel + 1]);
                break;
            case InventoryContainer.FrontBasket:
                dimmed.Add(frontBasket);
                break;
            case InventoryContainer.RearBasket:
                dimmed.Add(rearBasket);
                break;
            case InventoryContainer.SaddleBags:
                dimmed.Add(leftSaddleBag);
                dimmed.Add(rightSaddleBag);
                break;
            default:
                Debug.LogError("Unrecognized InventoryContainer: " + container);
                break;
        }

        foreach (var go in lastDimmed)
        {
            if (!dimmed.Contains(go))
            {
                go.GetComponent<Image>().color = backgroundColors[go];
            }
        }

        foreach (var go in dimmed)
        {
            go.GetComponent<Image>().color = dimmedColors[go];
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < messengerBags.Count; i++)
        {
            messengerBags[i].SetActive(i == GameManager.instance.messengerBagLevel);
        }
        frontBasket.SetActive(GameManager.instance.hasFrontBasket);
        rearBasket.SetActive(GameManager.instance.hasRearBasket);
        leftSaddleBag.SetActive(GameManager.instance.hasSaddlebags);
        rightSaddleBag.SetActive(GameManager.instance.hasSaddlebags);

        foreach (GameObject go in dimmed)
        {
            go.SetActive(true);
        }
    }
}
