using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlate : input
{
    [SerializeField] Transform onPosition;
    [SerializeField] Transform offPosition;
    [SerializeField] float animSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, onPosition.localScale, animSpeed * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, offPosition.localScale, animSpeed * Time.deltaTime);
        }
        Activate();
    }

    private void OnTriggerEnter(Collider other)
    {
        isOn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isOn = false;
    }
}
