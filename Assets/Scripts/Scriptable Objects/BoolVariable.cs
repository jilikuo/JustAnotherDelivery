using UnityEngine;
using System;

[CreateAssetMenu]
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