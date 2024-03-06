using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : input
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Activate();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            isOn = true;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if(collision.tag == "Player")
        {
            isOn = false;
        }
    }
}
