using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedButton : input
{
    [SerializeField] Transform sphere;
    [SerializeField] Transform onPosition;
    [SerializeField] Transform offPosition;
    [SerializeField] float animSpeed;
    bool canActivate;
    bool inRange;
    [SerializeField] float buttonTime;

    // Start is called before the first frame update
    void Start()
    {
        onPosition = transform.GetChild(0);
        offPosition = transform.GetChild(1);
        canActivate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && inRange)
        {
            if (Input.GetKeyDown("e"))
            {
                isOn = true;
                canActivate = false;
                
                StartCoroutine(buttonCoolDown());
            }
        }
        if (isOn)
        {
            sphere.localScale = Vector3.Lerp(sphere.localScale, onPosition.localScale, animSpeed* Time.deltaTime);
        }
        else
        {
            sphere.localScale = Vector3.Lerp(sphere.localScale, offPosition.localScale, animSpeed * Time.deltaTime);
        }
        Activate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            inRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

    public IEnumerator buttonCoolDown()
    {
        yield return new WaitForSeconds(buttonTime);
        isOn = false;
        canActivate = true;
    }
}
