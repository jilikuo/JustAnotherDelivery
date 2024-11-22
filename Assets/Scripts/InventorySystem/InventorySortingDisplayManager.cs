using DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySortingDisplayManager : MonoBehaviour
{
    [Header("Required fields")]
    [SerializeField] private List<GameObject> messengerBags;
    [SerializeField] private GameObject frontBasket;
    [SerializeField] private GameObject rearBasket;
    [SerializeField] private GameObject leftSaddleBag;
    [SerializeField] private GameObject rightSaddleBag;

    // Start is called before the first frame update
    private void Start()
    {
        if (GameManager.instance.numMessengerBagLevels != messengerBags.Count)
        {
            Debug.LogError("numMessengerBagLevels(" + GameManager.instance.numMessengerBagLevels + ") does not equal messengerBags count(" + messengerBags.Count + ")");
        }

        UpdateDisplay();
        Action<GameObject> AddContainerMananager = go =>
        {
            if (go.GetComponentInChildren<InventorySortingContainerManager>() == null)
            {
                go.AddComponent<InventorySortingContainerManager>();
            }
        };

        for (int i = 0; i < messengerBags.Count; i++)
        {
            AddContainerMananager(messengerBags[i]);
        }
        AddContainerMananager(frontBasket);
        AddContainerMananager(rearBasket);
        AddContainerMananager(leftSaddleBag);
        AddContainerMananager(rightSaddleBag);
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
    }
}
