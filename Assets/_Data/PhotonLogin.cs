using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonLogin : MonoBehaviour
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

        PhotonNetwork.LocalPlayer.NickName = name;
        PhotonNetwork.ConnectUsingSettings();
    }
}
