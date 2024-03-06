using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScript
{
    
    public List<float> floatValues;
    public List<string> floatKeys;

    public List<bool> boolValues;
    public List<string> boolKeys;

    public List<Vector3> v3Values;
    public List<string> v3Keys;

    public DataScript()
    {
        floatValues = new List<float>();
        floatKeys = new List<string>();

        boolValues = new List<bool>();
        boolKeys = new List<string>();

        v3Values = new List<Vector3>();
        v3Keys = new List<string>();
    }
}
