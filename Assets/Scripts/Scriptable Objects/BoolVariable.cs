using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Utilities/Bool Variable")]
[Serializable]
public class BoolVariable : ScriptableObject
{
    public bool value = false;

    public BoolVariable()
    {
    }

    public BoolVariable(bool val)
    {
        value = val;
    }
}