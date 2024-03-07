using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class Listing : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI text;
    public string roomName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(RoomInfo info)
    {
        text.text = $"{info.Name}: <color=grey>{info.PlayerCount}/{info.MaxPlayers}</color>";
        roomName = info.Name;
    }

    public void clickedJoin()
    {
        roomManager.instance.joinRoom(roomName);
    }
}
