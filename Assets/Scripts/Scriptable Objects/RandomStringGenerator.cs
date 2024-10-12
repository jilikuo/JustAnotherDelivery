using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu]
[Serializable]
public class RandomStringGenerator : RandomObjectGenerator<string>
{
    protected override string GetKey(string entry)
    {
        return entry;
    }

    protected override int IndexOf(string key)
    {
        return entries.IndexOf(key);
    }

    protected override string CopyEntry(string entry)
    {
        return string.Copy(entry);
    }
}