using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Characters")]
[Serializable]
public class Characters : ScriptableObject
{
    public string fullName;
    public string title;
    public GameObject characterPrefab;

    public bool hasBeenAssigned; // just for in-editor QoL
}
