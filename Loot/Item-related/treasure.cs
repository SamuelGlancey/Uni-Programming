using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasure : MonoBehaviour
{
    public float treasureHealth;
    bool canBeDug;
    PlayerController_ player;
    // Start is called before the first frame update
    void Start()
    {
        canBeDug = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(treasureHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void whenDug(float damage)
    {
        if (canBeDug)
        {
            treasureHealth -= damage;
            StartCoroutine(diggingInterval());
        }
    }

    public IEnumerator diggingInterval()
    {
        canBeDug = false;
        yield return new WaitForSeconds(0.5f);
        canBeDug = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.GetComponent<PlayerController_>();
            player.digSomething += whenDug;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController_>().digSomething -= whenDug;
        }
    }

    private void OnDisable()
    {
        player.digSomething -= whenDug;
    }
}
