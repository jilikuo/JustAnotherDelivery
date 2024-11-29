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
    [Header("Required fields")]
    [SerializeField] protected List<GameObject> messengerBags;
    [SerializeField] protected GameObject frontBasket;
    [SerializeField] protected GameObject rearBasket;
    [SerializeField] protected GameObject leftSaddleBag;
    [SerializeField] protected GameObject rightSaddleBag;

    protected List<GameObject> containers = new List<GameObject>();

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

        UpdateDisplay();

        AfterStart();
    }

    protected virtual void AfterStart()
    {
    }

    public virtual void UpdateDisplay()
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
