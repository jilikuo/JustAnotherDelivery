using DragDrop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RecycleItemDisplayManager : MonoBehaviour, IItemDroppable
{
    [SerializeField] private Color highlightColor = new Color(0f, 1f, 0f, .40f);
    // TODO: Remove itemText, if it will not be used in final UI
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private GameObject itemBackground;
    [SerializeField] private InventorySortingPackageGenerator packageGenerator;
    [SerializeField] private float recycleTime = 1f;

    private Image backgroundImage;
    private Color backgroundColor;

    private void Awake()
    {
        if (itemText == null)
        {
            Debug.LogError("itemText is not set");
        }
        if (itemBackground == null)
        {
            Debug.LogError("itemBackground is not set");
        }
        if (packageGenerator == null)
        {
            Debug.LogError("packageGenerator is not set");
        }
    }

    private void Start()
    {
        if (itemText != null)
        {
            itemText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        backgroundImage = itemBackground.GetComponent<Image>();
        backgroundColor = backgroundImage.color;
    }

    private void Update()
    {
        var item = DragDropObject.currentDragDropObject;
        if (item == null)
        {
            ClearHighlight();
            return;
        }
        if (!backgroundImage.GetWorldBounds().Intersects(item.GetWorldBounds()))
        {
            ClearHighlight();
            return;
        }
        SetHighlight();
    }

    public void SetHighlight()
    {
        backgroundImage.color = highlightColor;
    }


    public void ClearHighlight()
    {
        backgroundImage.color = backgroundColor;
    }

    private IEnumerator OnFinishRecycle(DragDropObject item)
    {
        var time = recycleTime;
        var icon = item.GetComponent<Image>();
        Color color = icon.color;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            color.a = time / recycleTime;
            icon.color = color;
            yield return null;
        }

        item.transform.SetParent(null);
        packageGenerator.ReturnDragDrop(item);
    }

    public void AddDragDropObject(DragDropObject item)
    {
        if (item  == null)
        {
            return;
        }

        item.transform.SetParent(itemBackground.transform);
        item.transform.position = itemBackground.transform.position;
        StartCoroutine(OnFinishRecycle(item));
    }

    public bool IsValidDropPosition(DragDropObject item)
    {
        return true;
    }
}
