using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MinimapInterestPoints : MonoBehaviour
{
    public List<GameObject> interestPoints;

    private List<GameObject> activeInterestPoints = new();
    private Inventory inventory;
    private bool hasActiveInterestPoints;
    private bool isDirty = false;
    private int lastInventoryCount = 0;

    private void Start()
    {
        if (interestPoints != null)
        {
            foreach (var interstPoint in interestPoints)
            {
                interstPoint.SetActive(false);
                hasActiveInterestPoints = false;
            }
        }

        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("MinimapIntPoint needs an Inventory object to work with");
        }
    }

    private void Update()
    {
        if (inventory.packages.Count != lastInventoryCount)
        {
            RefreshActiveList();
        }

        if (hasActiveInterestPoints && isDirty)
        {
            UpdateInterestPointState();
        }
    }

    private void RefreshActiveList()
    {
        lastInventoryCount = inventory.packages.Count;

        if (inventory.packages == null)
        {
            foreach (var interestPoint in interestPoints)
            {
                if (interestPoint.activeSelf)
                {
                    interestPoint.SetActive(false);
                }
            }
            return;
        }

        activeInterestPoints.Clear();
        foreach (var package in inventory.packages)
        {
            Match match = Regex.Match(package.address.address, @"\d+");

            if (!match.Success)
            {
                continue;
            }

            foreach (var interestPoint in interestPoints)
            {
                if (int.Parse(interestPoint.name) == int.Parse(match.Value))
                {
                    activeInterestPoints.Add(interestPoint);
                }
            }
        }

        if (activeInterestPoints.Count > 0)
        {
            hasActiveInterestPoints = true;
        }
        else
        {
            hasActiveInterestPoints = false;
        }

        isDirty = true;
    }

    private void UpdateInterestPointState()
    {
        foreach(var interestPoint in interestPoints)
        {   
            if (activeInterestPoints.Contains(interestPoint))
            {
                interestPoint.SetActive(true);
            }
            else
            {
                interestPoint.SetActive(false);
            }
        }

        isDirty = false;
    }
}
