using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] Transform openPos;
    [SerializeField] Transform closedPos;
    [SerializeField] float t;
    public bool locked;
    protected bool isOpen;
    void Start()
    {
        
        locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkIfOpen();

    }

    protected void checkIfOpen()
    {
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPos.position, t * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPos.position, t * Time.deltaTime);
        }
    }
}
