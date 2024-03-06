using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<healthController>())
        {
            other.GetComponent<healthController>().takeDamage(other.GetComponent<healthController>().MaxHealth);
        }
    }
}
