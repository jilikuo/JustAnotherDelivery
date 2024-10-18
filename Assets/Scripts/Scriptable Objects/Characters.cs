using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Characters")]
public class Characters : ScriptableObject
{
    public string fullName;
    public string title;
    public GameObject characterPrefab;
}
