using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoomAuto : MonoBehaviourPunCallbacks
{
    public string nickName = "Nick";
    public string roomName = "Room";

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
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(transform.name + ": OnJoinedLobby");
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 7
        };
        PhotonNetwork.CreateRoom(this.roomName, roomOptions);
    }
}
