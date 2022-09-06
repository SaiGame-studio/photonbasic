using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonLogout : MonoBehaviour
{

    public virtual void Logout()
    {
        Debug.Log(transform.name + ": Logout ");
        PhotonNetwork.Disconnect();

    }
}
