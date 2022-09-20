using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonNumber : MonoBehaviourPun, IPunObservable
{
    public TextMeshPro textNumber;
    public int number = 0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView");
        if (stream.IsWriting) this.StreamWriting(stream);
        else this.StreamReading(stream, info);
    }

    protected virtual void StreamWriting(PhotonStream stream)
    {
        Debug.Log("StreamWriting");
        stream.SendNext(this.number);
    }

    protected virtual void StreamReading(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("StreamReading");
        this.number = (int)stream.ReceiveNext();
        this.textNumber.text = this.number.ToString();
    }

    public virtual void Set(int number)
    {
        this.number = number;
        this.textNumber.text = number.ToString();
    }

    public virtual void OnClaim()
    {
        Debug.Log(transform.name+" OnClaim: " + this.number);
        object[] datas = new object[] { this.number };

        // You would have to set the Receivers to All in order to receive this event on the local client as well
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onNumberClaimed), 
            datas,
            raiseEventOptions,
            SendOptions.SendUnreliable
            );
    }

    internal void Claimed()
    {
        Debug.Log("Claimed: " + this.number);
        gameObject.SetActive(false);
    }
}
