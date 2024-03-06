using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class automaticDoor : door
{
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, player.position) < 10)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
        if (locked)
        {
            isOpen = false;
        }
        checkIfOpen();
    }
}
