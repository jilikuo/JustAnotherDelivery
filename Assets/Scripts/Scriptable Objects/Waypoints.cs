using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction System/Waypoints")]
[Serializable]
public class Waypoints : ScriptableObject
{
    [Serializable]
    public struct Coordinates
    {
        public float x;
        public float y;
        public float z;

        public Coordinates(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [SerializeField]private StreetName streetName;
    [SerializeField]private int addressNumber;

    public Coordinates position;
    public Coordinates rotation;

    public Waypoints northWaypoint;
    public Waypoints eastWaypoint;
    public Waypoints southWaypoint;
    public Waypoints westWaypoint;

    public bool isValid;
    public bool isAccessible;
    public bool isDirty;

    public List<Characters> residents;

    public string GetFullAddress()
    {
        return addressNumber + ", " + streetName.Name;
        //TODO: Extra Info (apt number, floor, etc)
    }

    public string GetStreetName()
    {
        return streetName.Name;
    }

    public int GetAddressNumber()
    {
        return addressNumber;
    }

    public bool IsNpcAvailable()
    {
        return residents.Count > 0;
    }

    private void OnValidate()
    {
        if (residents == null)
        {
            isDirty = false;
        }
        isDirty = true;
    }
}