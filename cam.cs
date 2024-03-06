using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().backgroundColor = gameManager.BackgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Camera>().backgroundColor = gameManager.BackgroundColor;
    }
}
