using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isPaused)
        {
            GetComponent<SpriteRenderer>().color = gameManager.BackgroundColor;
        }
    }
}
