using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public static PhotonRoom instance;

    public TMP_InputField input;
    public Transform roomContent;
    public UIRoomProfile roomPrefab;
    public bool autoStartGame = false;
    public List<RoomInfo> updatedRooms;
    public List<RoomProfile> rooms = new List<RoomProfile>();

    protected void Awake()
    {
        PhotonRoom.instance = this;//Dont do this in your game
    }

    protected void Start()
    {
        this.input.text = "Room1";
    }

    public virtual void Create()
    {
        string name = input.text;
        Debug.Log(transform.name + ": Create Room " + name);

        Debug.Log(transform.name + ": OnJoinedLobby");
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 7
        };
        PhotonNetwork.CreateRoom(name, roomOptions);
    }

    public virtual void Join()
    {
        string name = input.text;
        Debug.Log(transform.name + ": Join Room " + name);
        PhotonNetwork.JoinRoom(name);
        this.ClearRoomProfileUI();
    }

    public virtual void Leave()
    {
        Debug.Log(transform.name + ": Leave Room");
        PhotonNetwork.LeaveRoom();
    }

    public virtual void ClickStartGame()
    {
        Debug.Log(transform.name + ": Click Start Game");
        if (PhotonNetwork.IsMasterClient) this.StartGame();
        else Debug.LogWarning("You are not Master Client");
    }

    public virtual void StartGame()
    {
        Debug.Log(transform.name + ": Start Game");
        PhotonNetwork.LoadLevel("2_PhotonGame");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(transform.name + ": OnCreatedRoom");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(transform.name + ": OnJoinedRoom");
        if(this.autoStartGame) this.StartGame();
    }

    public override void OnLeftRoom()
    {
        Debug.Log(transform.name + ": OnLeftRoom");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(transform.name + ": OnCreateRoomFailed: " + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log(transform.name + ": OnRoomListUpdate");
        this.updatedRooms = roomList;

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) this.RoomRemove(roomInfo);
            else this.RoomAdd(roomInfo);
        }

        this.UpdateRoomProfileUI();
    }

    protected virtual void RoomAdd(RoomInfo roomInfo)
    {
        RoomProfile roomProfile;

        roomProfile = this.RoomByName(roomInfo.Name);
        if (roomProfile != null) return;

        roomProfile = new RoomProfile
        {
            name = roomInfo.Name
        };
        this.rooms.Add(roomProfile);
    }

    protected virtual void UpdateRoomProfileUI()
    {
        this.ClearRoomProfileUI();

        foreach (RoomProfile roomProfile in this.rooms)
        {
            UIRoomProfile uiRoomProfile = Instantiate(this.roomPrefab);
            uiRoomProfile.SetRoomProfile(roomProfile);
            uiRoomProfile.transform.SetParent(this.roomContent);
        }
    }

    protected virtual void ClearRoomProfileUI()
    {
        foreach (Transform child in this.roomContent)
        {
            Destroy(child.gameObject);
        }
    }

    protected virtual void RoomRemove(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = this.RoomByName(roomInfo.Name);
        if (roomProfile == null) return;
        this.rooms.Remove(roomProfile);
    }

    protected virtual RoomProfile RoomByName(string name)
    {
        foreach (RoomProfile roomProfile in this.rooms)
        {
            if (roomProfile.name == name) return roomProfile;
        }
        return null;
    }
}