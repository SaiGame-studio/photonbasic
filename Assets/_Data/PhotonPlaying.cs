using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonPlaying : MonoBehaviourPunCallbacks
{
    public static PhotonPlaying instance;

    protected void Awake()
    {
        PhotonPlaying.instance = this;//Dont do this in your game
    }

    public virtual void Leave()
    {
        Debug.Log(transform.name + ": Leave Room");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log(transform.name + ": OnLeftRoom");
        PhotonNetwork.LoadLevel("1_PhotonRoom");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom: " + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom: " + otherPlayer.NickName);
    }
}