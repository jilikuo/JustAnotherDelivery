using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public static class BoundsExtension
{
    public static bool ContainsBounds(this Bounds bounds, Bounds other)
    {
        return bounds.Contains(other.min) && bounds.Contains(other.max);
    }
}

public static class GameObjectExtension
{
    public static Bounds GetWorldBounds(this GameObject gameObject)
    {
        return gameObject.GetComponent<RectTransform>().ToWorldBounds();
    }

    public static void CenterAndStretchToParent(this GameObject gameObject)
    {
        // TODO: Determine if position needs to be set
        RectTransform rect = gameObject.GetComponent<RectTransform>();

        // Remove scaling
        rect.localScale = Vector3.one;

        // Set pivot to center
        rect.pivot = new Vector2(0.5f, 0.5f);

        // Set anchors to extents
        rect.anchorMin = new Vector2(0f, 0f); // Bottom-left corner
        rect.anchorMax = new Vector2(1f, 1f); // Top-right corner      

        // Reset offsets
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}

public static class ImageExtension
{
    public static Bounds GetWorldBounds(this Image image)
    {
        return image.rectTransform.ToWorldBounds();
    }
}

public static class RectTransformExtension
{
    public static void Copy(this RectTransform rect, RectTransform other)
    {
        rect.anchorMin = other.anchorMin;
        rect.anchorMax = other.anchorMax;
        rect.anchoredPosition = other.anchoredPosition;
        rect.sizeDelta = other.sizeDelta;
    }

    public static Bounds ToWorldBounds(this RectTransform rect)
    {
        Vector3[] v = new Vector3[4];
        rect.GetWorldCorners(v);
        return new Bounds((v[2] + v[0]) / 2, v[2] - v[0]);
    }
}

public static class ScriptableObjectExtension
{
    public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null) return null;

        T instance = UnityEngine.Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name; // remove (Clone) from name
        return instance;
    }
}
