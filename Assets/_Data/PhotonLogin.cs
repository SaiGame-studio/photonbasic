using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputUsername;

    void Start()
    {
        this.inputUsername.text = "Sai1";
    }

    public virtual void Login()
    {
        string name = inputUsername.text;
        Debug.Log(transform.name + ": Login " + name);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LocalPlayer.NickName = name;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }
}
