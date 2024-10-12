using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu]
[Serializable]
public class RandomGameObjectGenerator : RandomObjectGenerator<GameObject>
{
    protected override string GetKey(GameObject entry)
    {
        return (entry == null) ? null : entry.name;
    }

    protected override int IndexOf(string key)
    {
        return entries.FindIndex(x => x.name == key);
    }

    protected override GameObject CopyEntry(GameObject entry)
    {
        return Instantiate(entry);
    }
}
