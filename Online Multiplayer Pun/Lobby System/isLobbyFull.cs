using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

public class isLobbyFull : MonoBehaviour
{
    PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC("areYouHere", RpcTarget.Others);
    }
    [PunRPC]
    void areYouHere()
    {
        pv.RPC("hereSir", RpcTarget.Others);
    }
    [PunRPC]
    void hereSir()
    {
        pv.RPC("startGame", RpcTarget.All);
    }

    [PunRPC]
    void startGame()
    {
        //if (!isRPC)
        //{
        //    pv.RPC("startGame", RpcTarget.Others, true);
        //}
        Debug.LogError("elloooooooooooooooooooooooooooo");
        SceneManager.LoadScene("Game");
    }
}
