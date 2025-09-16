using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginData
{
    public string[] name;
    public string[] time;
    public float musicValue, soundValue;
    public void Init()
    {
        name = new string[6];
        time = new string[6];
        musicValue = 50;
        soundValue = 50;
    }
}
