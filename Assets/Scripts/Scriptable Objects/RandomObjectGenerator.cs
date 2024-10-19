using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class RandomObjectGenerator<T> : ScriptableObject where T : class
{
    private static System.Random rng = new System.Random();

    public T defaultEntry;
    public List<T> entries;

    public void Clear()
    {
        defaultEntry = null;
        entries = new List<T> ();
    }

    // Returns a copy of random entry
    public T GetEntryCopy()
    {
        var entry = GetEntry();
        return (entry == null) ? entry : CopyEntry(entry);
    }

    // Returns a random entry
    public T GetEntry()
    {
        if (entries.Count > 0)
        {
            return entries[rng.Next(entries.Count)];
        }
        else
        {
            return defaultEntry;
        }
    }

    // Returns a copy of the entry with the given key
    public T GetEntryCopy(string key)
    {
        var entry = GetEntry(key);
        return (entry == null) ? entry : CopyEntry(entry);
    }

    // Returns the entry with the given key
    public T GetEntry(string key)
    {
        if (key == null)
        {
            Debug.LogWarning("Attempted to get entry with null key");
            return null;
        }
        int index = IndexOf(key);
        return (index >= 0) ? entries[index] : defaultEntry;
    }

    // Removes the entry with a matching key
    public void AddEntry(T entry)
    {
        if (entry == null)
        {
            Debug.LogWarning("Attempted to add null entry");
            return;
        }
        entries.Add(entry);
    }

    // Removes the entry with a matching key
    public void RemoveEntry(T entry)
    {
        if (entry == null)
        {
            Debug.LogWarning("Attempted to remove null entry");
            return;
        }
        RemoveEntry(GetKey(entry));
    }

    // Removes the entry with the given key
    public void RemoveEntry(string key)
    {
        if (key == null)
        {
            Debug.LogWarning("Attempted to remove null entry");
            return;
        }
        int index = IndexOf(key);
        if (index >= 0)
        {
            entries.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Failed to locate entry with key: " + key);
        }
    }

    protected abstract int IndexOf(string key);

    protected abstract string GetKey(T entry);

    protected abstract T CopyEntry(T entry);
}
