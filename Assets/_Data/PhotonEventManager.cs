using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class PhotonEventManager : MonoBehaviour
{
    protected void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += this.EventReceived;
    }

    protected void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= this.EventReceived;
    }

    private void EventReceived(EventData obj)
    {
        Debug.Log("EventReceived:" + obj.Code.ToString());
        if (obj.Code == (byte)EventCode.onNumberClaimed) this.OnEventNumberClaimed(obj);
    }

    private void OnEventNumberClaimed(EventData obj)
    {
        object[] datas = (object[])obj.CustomData;
        int number = (int) datas[0];
        Debug.Log("OnNumberClaimed: " + number);
        GameManager.instance.NumberOnClaimed(number);

    }
}
