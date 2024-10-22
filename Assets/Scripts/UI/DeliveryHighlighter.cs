using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryHighlighter : MonoBehaviour
{
    public GameObject deliveryBoxHighlighter;
    public float colorChangeSpeed = 0.6f;
    public float colorizationRatio = 1.5f;

    private Inventory inventory;
    private NavigationMenuHandler navigationManager;
    private Image deliveryBoxImage;

    private Color baseColor;
    private Color highlightColor;
    private bool whitening = true;
    private float transitionProgress = 0;

    void Start()
    {
        if (deliveryBoxHighlighter == null)
        {
            Debug.LogError("DeliveryHighlighter: deliveryBoxHighlighter is not set.");
        }

        deliveryBoxImage = deliveryBoxHighlighter.GetComponent<Image>();

        if (deliveryBoxHighlighter.activeSelf)
        {
            deliveryBoxHighlighter.SetActive(false);
        }

        inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        navigationManager = GameObject.FindGameObjectWithTag("NavigationManager").GetComponent<NavigationMenuHandler>();

        highlightColor = deliveryBoxImage.color;
        baseColor =  new Color(Mathf.Min(highlightColor.r * colorizationRatio, 255),
                               Mathf.Min(highlightColor.g * colorizationRatio, 255),
                               Mathf.Min(highlightColor.b * colorizationRatio, 255),
                               100);
    }

    private void Update()
    {
        if (!navigationManager.IsDeliveryBoxAvailable())
        {
            if (deliveryBoxHighlighter.activeSelf)
            {
                deliveryBoxHighlighter.SetActive(false);
            }

            return;
        }

        bool isHighlighted = CheckHighlight();
        if (isHighlighted != deliveryBoxHighlighter.activeSelf)
        {
            deliveryBoxHighlighter.SetActive(isHighlighted);
            if (isHighlighted)
            {
                AnimateHighlighter();
            }
        }
    }

    private void AnimateHighlighter()
    {
        transitionProgress += colorChangeSpeed * Time.deltaTime;

        if (transitionProgress > 1f)
        {
            transitionProgress = 1f;
        }

        if (whitening)
        {
            deliveryBoxImage.color = Color.Lerp(highlightColor, baseColor, transitionProgress);
        }
        else
        {
            deliveryBoxImage.color = Color.Lerp(baseColor, highlightColor, transitionProgress);
        }

        if (transitionProgress >= 1f)
        {
            whitening = !whitening;
            transitionProgress = 0;
        }
    }

    private bool CheckHighlight()
    {
        if (!navigationManager.IsPlayerInteracting())
        {
            return false;
        }

        foreach (var package in inventory.packages)
        {
            if (package.address.fullName.Equals(navigationManager.GetActiveNPC().fullName))
            {
                return true;
            }
        }

        return false;
    }
}   
