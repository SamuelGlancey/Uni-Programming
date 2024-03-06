using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wire : MonoBehaviour
{
    public bool isOn;
    public bool isStartingWire;
    public float delay;
    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;
    [SerializeField]wireReciever reciever;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            GetComponent<MeshRenderer>().material = onMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = offMaterial;
        }
        if (!isStartingWire) 
        {
            StartCoroutine(delayWire(reciever.hasRecievedPositiveSignal));
        }
    }

    public IEnumerator delayWire(bool signalRecieved)
    {
        yield return new WaitForSeconds(delay);
        isOn = signalRecieved;
    }
}
