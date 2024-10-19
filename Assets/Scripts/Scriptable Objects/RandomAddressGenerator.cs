using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Utilities/Random Address Generator")]
[Serializable]
public class RandomAddressGenerator : RandomObjectGenerator<Address>
{
    protected override string GetKey(Address entry)
    {
        return (entry == null) ? null : entry.ToString();
    }

    protected override int IndexOf(string key)
    {
        return entries.FindIndex(x => x.ToString() == key);
    }

    protected override Address CopyEntry(Address entry)
    {
        return entry.Clone();
    }
}
