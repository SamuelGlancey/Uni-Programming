using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//set colour of button to be the background colour. 
public class buttonColourBG : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().Select();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = gameManager.BackgroundColor;
    }
}
