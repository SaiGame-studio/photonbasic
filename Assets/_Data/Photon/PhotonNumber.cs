
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PhotonNumber : MonoBehaviourPun, IPunObservable
{
    public TextMeshPro textNumber;
    [SerializeField] protected int number = 0;

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
}
