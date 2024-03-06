using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checkpoint : MonoBehaviour
{
    [SerializeField] Transform startpoint;
    [SerializeField] door backDoor;
    [SerializeField] door frontDoor;
    public int room = 0;
    [SerializeField] List<GameObject> roomList;
    [SerializeField] Transform roomSpawn;
    [SerializeField] GameObject currentRoom;
    [SerializeField] Transform roomEnd;
    [SerializeField] exitedTransitionRoom exit;
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
            //increment level
            room++;
            //parent player to room
            other.transform.parent = transform.root;
            //move room to the start
            transform.root.position = startpoint.position;
            //lock back door
            backDoor.locked = true;
            //unlock front door
            frontDoor.locked = false;
            //unload level
            if(currentRoom != null)
            {
                Destroy(currentRoom);
            }
            //load level
            currentRoom = Instantiate(roomList[room], roomSpawn);
            roomEnd = currentRoom.GetComponent<room>().roomEnd;
            exit.backDoor.locked = false;
            gameObject.SetActive(false);
        }
    }
    public void moveTransitionRoom()
    {
        gameObject.SetActive(true);
        //move room to end
        transform.root.position = roomEnd.position;
        //lock front door
        frontDoor.locked = true;
        //unlock back door#
        backDoor.locked = false;
    }

}
