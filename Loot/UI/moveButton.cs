using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveButton : MonoBehaviour
{
    public bool toggleButton;
    public Sprite onSprite;
    public Sprite offSprite;
    public PlayerController_ pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController_>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.GetIsDigging())
        {
            toggleButton = false;
        }
        if(GameManager.currentState != GameManager.GameState.GAMEPLAY)
        {
            toggleButton = false;
        }
        if (pc.GetIsBackstepping())
        {
            toggleButton = false;
        }
        if (toggleButton)
        {
            GetComponent<Image>().sprite = onSprite;
            pc.move();
            pc.SetIsMoving(true);
        }
        else
        {
            GetComponent<Image>().sprite = offSprite;
            pc.SetIsMoving(false);
        }
    }

    public void SetToggle()
    {
        pc.SetIsDigging(false);
        pc.SetIsBackstepping(false);
        toggleButton = !toggleButton;
    }

}
