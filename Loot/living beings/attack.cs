using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public float damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        healthController hc = other.GetComponentInChildren<healthController>();
        if (hc != null)
        {
            hc.takeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
