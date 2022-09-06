using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAutoStartGame : MonoBehaviour
{
    public Toggle toggle;

    public virtual void OnChanged()
    {
        Debug.Log("On Changed");
        PhotonRoom.instance.autoStartGame = this.toggle.isOn;
    }
}