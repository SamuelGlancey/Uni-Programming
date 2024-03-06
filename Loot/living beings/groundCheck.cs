using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public PlayerController_ player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "Player")
        {
            player.SetIsGrounded(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            player.SetIsGrounded(false);
        }
    }
}
