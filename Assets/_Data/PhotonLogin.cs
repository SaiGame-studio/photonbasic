using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputUsername;
    public string nickName;

    void Start()
    {
        this.nickName = "Sai";
        this.inputUsername.text = this.nickName;
    }

    public virtual void OnChangeName()
    {
        this.nickName = this.inputUsername.text;
    }

    public virtual void Login()
    {
        string name = this.nickName;
        Debug.Log(transform.name + ": Login " + name);

        //PhotonNetwork.SendRate = 20;
        //PhotonNetwork.SerializationRate = 5;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LocalPlayer.NickName = name;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(transform.name + ": OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }
}
