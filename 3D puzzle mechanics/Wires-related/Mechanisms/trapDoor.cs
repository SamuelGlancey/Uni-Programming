using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapDoor : MonoBehaviour
{
    bool isActive;
    [SerializeField]Transform open;
    [SerializeField]Transform closed;
    [SerializeField]float speed;
    [SerializeField]wireReciever reciever;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isActive = reciever.hasRecievedPositiveSignal;
        if (isActive)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, closed.rotation, speed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, open.rotation, speed * Time.deltaTime);

        }
    }
}
