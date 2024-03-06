using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitedTransitionRoom : MonoBehaviour
{
    public door backDoor;
    [SerializeField] checkpoint checkpoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            backDoor.locked = true;
            other.transform.parent = null;
            StartCoroutine(MoveTransitionRoom());
        }
    }

    public IEnumerator MoveTransitionRoom()
    {
        yield return new WaitForSeconds(0.5f);
        checkpoint.moveTransitionRoom();

    }
}
