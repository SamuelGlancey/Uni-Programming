using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golden_bog : MonoBehaviour
{
    public List<GameObject> goldenObjects;
    int amountUnlocked;
    public Animator anim;
    public GameObject boss;
    public GameObject looBlocker;
    bool canBeSatOn;
    // Start is called before the first frame update

    private void OnEnable()
    {
        PlayerController_.GoldenCollected += unlockObject;
    }

    private void OnDisable()
    {
        PlayerController_.GoldenCollected -= unlockObject;
    }
    public void unlockObject(int goldenObject)
    {
        goldenObjects[goldenObject].SetActive(true);
        amountUnlocked++;
        if(amountUnlocked == 3)
        {
            canBeSatOn = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController_ pc = other.GetComponent<PlayerController_>();
            if (canBeSatOn)
            {
                if (pc.GetShook())
                {
                    pc.SetNearestToilet(transform);
                    pc.sitOnToilet();
                    boss.SetActive(true);
                    looBlocker.SetActive(false);
                    anim.SetTrigger("finish");
                }
            }
        }
    }
}
