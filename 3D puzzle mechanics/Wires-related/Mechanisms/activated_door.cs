using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activated_door : door
{
    [SerializeField] wireReciever reciever;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (reciever.hasRecievedPositiveSignal)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
        checkIfOpen();
    }
}
