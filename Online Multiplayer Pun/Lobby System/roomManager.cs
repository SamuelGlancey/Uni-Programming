using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class roomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInput;
    public TMP_InputField searchInput;
    public GameObject roomListing;
    public Transform roomLayout;
    public List<GameObject> currentRooms;
    public Toggle isPrivate;
    public static roomManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PhotonNetwork.JoinLobby();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject room in currentRooms)
        {
            if (searchInput == null || room.GetComponent<Listing>().roomName.ToLower().Contains(searchInput.text.ToLower()))
            {
                room.SetActive(true);
            }
            else
            {
                room.SetActive(false);
            }
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        foreach(RoomInfo info in roomList)
        {
            for (int i = 0; i < currentRooms.Count; i++)
            {
                if (info.Name == currentRooms[i].GetComponent<Listing>().roomName)
                {
                    Destroy(currentRooms[i]);
                    currentRooms.RemoveAt(i);
                }
            }

            if (info.IsVisible)
            {
                var room = Instantiate(roomListing, roomLayout);
                room.GetComponent<Listing>().SetInfo(info);
                currentRooms.Add(room);
            }
        }
    }
    public void createRoom()
    {
        RoomOptions options = new RoomOptions();
        options.IsVisible = !isPrivate.isOn;
        options.MaxPlayers = 2;
        if(roomNameInput.text != "")
        {
            PhotonNetwork.CreateRoom(roomNameInput.text, options, TypedLobby.Default); 
        }
    }

    public void joinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameInput.text);
    }

    public void joinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("ready up");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("room created");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public void autoFillText(string text)
    {
        roomNameInput.text = text;
    }

    public void exitGame()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene("Title");
    }
}
