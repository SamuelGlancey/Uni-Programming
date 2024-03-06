using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireReciever : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasRecievedPositiveSignal;
    public float numberOfInputsRequired = 1;
    public List<wire> listOfInputs; 
    void Start()
    {
        listOfInputs = new List<wire>();
    }

    // Update is called once per frame
    void Update()
    {
        decideRecieverSignal();
    }
    public virtual void decideRecieverSignal()
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
            hasRecievedPositiveSignal = true;
        }
        else
        {
            hasRecievedPositiveSignal = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<wire>())
        {
            if (!listOfInputs.Contains(other.GetComponent<wire>()))
            {
                listOfInputs.Add(other.GetComponent<wire>());
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<wire>())
        {
            if (listOfInputs.Contains(other.GetComponent<wire>()))
            {
                listOfInputs.Remove(other.GetComponent<wire>());
            }

        }
    }
}
