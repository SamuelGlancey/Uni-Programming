using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class connectionManager : MonoBehaviourPunCallbacks
{
    bool canGoToTitle = true;
    float disconnectionTimer = 0;
    public GameObject connectionObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    //    if (SceneManager.GetActiveScene().name != "Title")
    //    {
    //        if (gameManager.currentMode == GameModes.MULTIPLAYER)
    //        {
    //            if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && canGoToTitle)
    //            {
    //                disconnectionTimer += Time.deltaTime;
    //                if (disconnectionTimer > 5)
    //                {
    //                    disconnectionTimer = 0;
    //                    canGoToTitle = false;
    //                    PhotonNetwork.Disconnect();
    //                    SceneManager.LoadScene("Title");
    //                }
    //            }
    //            else
    //            {
    //                disconnectionTimer = 0;
    //            }
    //        }
    //    }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (canGoToTitle)
        {
            SceneManager.LoadScene("Title");
        }
        canGoToTitle = true;

    }
    public void ConnectToInternet()
    {
        //
        Instantiate(connectionObject, GameObject.Find("Canvas").transform.GetChild(0).position, Quaternion.identity, GameObject.Find("Canvas").transform);
    }
}
