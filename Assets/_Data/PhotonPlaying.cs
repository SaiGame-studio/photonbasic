using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonPlaying : MonoBehaviourPunCallbacks
{
    public static PhotonPlaying instance;
    public string photonPlayerName = "PhotonPlayer";
    public List<PlayerProfile> players = new List<PlayerProfile>();

    protected void Awake()
    {
        PhotonPlaying.instance = this;//Dont do this in your game

        this.LoadRoomPlayers();
        this.SpawnPlayer();
    }

    protected virtual void SpawnPlayer()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Invoke("SpawnPlayer", 1f);
            return;
        }

        GameObject playerObj = PhotonNetwork.Instantiate(this.photonPlayerName, Vector3.zero, Quaternion.identity);
        PhotonView photonView = playerObj.GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            PhotonPlayer photonPlayer = playerObj.GetComponent<PhotonPlayer>();
            PhotonPlayer.me = photonPlayer;
        }
    }


    protected virtual void LoadRoomPlayers()
    {
        Debug.Log("LoadRoomPlayers");
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Invoke("LoadRoomPlayers", 1f);
            return;
        }

        PlayerProfile playerProfile;
        foreach (KeyValuePair<int, Player> playerData in PhotonNetwork.CurrentRoom.Players)
        {
            //Debug.Log(playerData.Value.NickName);
            playerProfile = new PlayerProfile
            {
                nickName = playerData.Value.NickName
            };
            this.players.Add(playerProfile);
        }
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