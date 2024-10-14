using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Streets
{
    FirstAvenue, //A1-A13
    SecondAvenue, //B2-B13
    FirstStreet, //D1-D7
    SecondStreet, //D10-D12
    FirstLane, //C1-D1
    SecondLane, //C4-D4
    HQWay, //B10-D10
    CentralStreet, //A7-D7 currently there are no buildings at this street
}

[Serializable]
public class StreetName
{
    public Streets streetId;
    private string name;

    private static Dictionary<Streets, string> streetNames = new Dictionary<Streets, string>
    {
        {Streets.FirstAvenue, "Completely Normal Avenue"},
        {Streets.SecondAvenue, "This Is Actually Fine Avenue"},
        {Streets.FirstStreet, "This Is Definitely A Street Street"},
        {Streets.SecondStreet, "Another Okay Street"},
        {Streets.FirstLane, "Why Come Here Lane"},
        {Streets.SecondLane, "Not A Street Lane"},
        {Streets.HQWay, "Far Far Away Way"},
        {Streets.CentralStreet, "Central Street"}
    };

    public StreetName(Streets streetId)
    {
        this.streetId = streetId;
        this.name = streetNames[streetId];
    }
}
