using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input : MonoBehaviour
{
    protected bool isOn;

    [SerializeField] wire startingWire;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    protected void Activate()
    {
        startingWire.isStartingWire = true;
        startingWire.isOn = isOn;
    }


}
