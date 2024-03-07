using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class ConnectToInternet : MonoBehaviourPunCallbacks
{

    // Start is called before the first frame update
     public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Lobby");
        }
        connect();
    }
    void connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {

    }
    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }

}
