using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public enum pickupType
    {
        HEALTH,
        BOGROLL
    }
    public pickupType item = pickupType.HEALTH;
    public float amount = 50f;
    public Vector3 velocity;
    public float vacuumSpeed;
    public Collider trigger;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().AddForce(velocity);
        Invoke("enableTrigger", 1f);
        InvokeRepeating("vacuum", 1.2f, 0.1f);
        
    }
    void enableTrigger()
    {
        trigger.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
    void vacuum()
    {
        rb.velocity = (GameObject.FindGameObjectWithTag("Player").transform.position - (transform.position + transform.up)).normalized * vacuumSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (item)
            {
                case pickupType.HEALTH:
                    other.GetComponent<healthController>().Heal(amount);
                    break;
                case pickupType.BOGROLL:
                    PlayerController_.rolls += (int)amount;
                    break;
            }

            Destroy(gameObject);
        }
    }
}
