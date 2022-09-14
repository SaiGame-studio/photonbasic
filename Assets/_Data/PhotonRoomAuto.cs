using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomAuto : MonoBehaviourPunCallbacks
{
    public string nickName = "AutoName";
    public string roomName = "AutoRoom";

    void Awake()
    {
        this.AutoLogin();
    }

    protected virtual void AutoLogin()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined) return;
        PhotonNetwork.LocalPlayer.NickName = this.nickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(transform.name + ": OnConnectedToMaster");
        PhotonNetwork.CreateRoom(this.roomName);
    }
}
