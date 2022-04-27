using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Flag
{
    public string country;
    public Texture flagImage;
    public Region region;
    [Range(1, 5)]
    public int size;


    public enum Region
    {
        ASIA,AFRICA,NORTHAMERICA,SOUTHAMERICA,AUSTRALIA, EUROPE
    }
}
