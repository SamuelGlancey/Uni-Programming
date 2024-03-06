using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Not_Reciever : wireReciever
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        decideRecieverSignal();
    }
    public override void decideRecieverSignal()
    {
        float positiveInputs = 0;
        foreach (wire wire in listOfInputs)
        {
            if (wire.isOn)
            {
                positiveInputs++;
            }
        }
        if (positiveInputs >= numberOfInputsRequired)
        {
            hasRecievedPositiveSignal = false;
        }
        else
        {
            hasRecievedPositiveSignal = true;
        }
    }
}
