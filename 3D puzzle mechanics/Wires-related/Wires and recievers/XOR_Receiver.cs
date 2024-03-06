using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XOR_Receiver : wireReciever
{
    [SerializeField] private int maxNumberOfInputs = 1;
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
        if (positiveInputs >= numberOfInputsRequired && positiveInputs <= maxNumberOfInputs)
        {
            hasRecievedPositiveSignal = true;
        }
        else
        {
            hasRecievedPositiveSignal = false;
        }
    }
}
