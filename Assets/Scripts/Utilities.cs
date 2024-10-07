using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public static class BoundsExtension
{
    public static bool ContainsBounds(this Bounds bounds, Bounds other)
    {
        return bounds.Contains(other.min) && bounds.Contains(other.max);
    }
}

public static class CollectionExtension
{
    private static System.Random rng = new System.Random();

    public static int RandomIndex<T>(this IList<T> list)
    {
        return rng.Next(list.Count);
    }
    public static T RandomElement<T>(this IList<T> list)
    {
        return list[list.RandomIndex()];
    }

    public static int RandomIndex<T>(this T[] array)
    {
        return rng.Next(array.Length);
    }
    public static T RandomElement<T>(this T[] array)
    {
        return array[array.RandomIndex()];
    }
}

public static class RectTransformExtension
{
    public static Bounds ToWorldBounds(this RectTransform rect)
    {
        Vector3[] v = new Vector3[4];
        rect.GetWorldCorners(v);
        return new Bounds((v[2] + v[0]) / 2, v[2] - v[0]);
    }
}

public static class GameObjectExtension
{
    public static Bounds GetWorldBounds(this GameObject gameObject)
    {
        return gameObject.GetComponent<RectTransform>().ToWorldBounds();
    }
}

public static class ImageExtension
{
    public static Bounds GetWorldBounds(this Image image)
    {
        return image.rectTransform.ToWorldBounds();
    }
}
