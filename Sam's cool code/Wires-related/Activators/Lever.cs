using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : input
{
    [SerializeField] Transform sphere;
    [SerializeField] Transform onPosition;
    [SerializeField] Transform offPosition;
    [SerializeField] float leverSpeed;
    bool canActivate;
    
    // Start is called before the first frame update
    void Start()
    {
        onPosition = transform.GetChild(0);
        offPosition = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate)
        {
            if (Input.GetKeyDown("e"))
            {
                isOn = !isOn;
                Activate();
            }
        }
        if (isOn)
        {
            sphere.rotation = Quaternion.Lerp(sphere.rotation, onPosition.rotation, leverSpeed *Time.deltaTime);
        }
        else
        {
            sphere.rotation = Quaternion.Lerp(sphere.rotation, offPosition.rotation, leverSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            canActivate = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canActivate = false;
    }
}
