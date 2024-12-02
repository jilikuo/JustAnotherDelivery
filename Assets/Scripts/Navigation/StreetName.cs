using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Streets
{
    //Neighborhood 01
    Lorryload,
    Haulage,
    Remittance,
    Freightage,
    Portage,
    Batch
}

[Serializable]
public class StreetName
{
    [SerializeField]private Streets streetId;
    private string name;
    public string Name
    {
        get 
        {
            return streetId.ToString(); 
        }
    }

    public StreetName(Streets streetId)
    {
        this.streetId = streetId;
        this.name = streetId.ToString();
    }
}
