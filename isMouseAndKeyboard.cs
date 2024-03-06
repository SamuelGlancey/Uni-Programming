using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isMouseAndKeyboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager.mouseAndKeyboard)
        {
            GetComponent<Toggle>().isOn = false;
        }

        else if (!gameManager.mouseAndKeyboard)
        {
            GetComponent<Toggle>().isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
