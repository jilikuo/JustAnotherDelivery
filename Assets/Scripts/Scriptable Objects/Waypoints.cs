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

    public StreetName streetName;
    public int addressNumber;

    public Coordinates position;
    public Coordinates rotation;

    public Waypoints northWaypoint;
    public Waypoints eastWaypoint;
    public Waypoints southWaypoint;
    public Waypoints westWaypoint;

    public bool isValid;
    public bool isAccessible;

    public List<Characters> residents;
}