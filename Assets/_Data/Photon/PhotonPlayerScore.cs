using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PhotonPlayerScore : MonoBehaviourPun, IPunObservable
{
    public static PhotonPlayer me;
    public PhotonView _photonView;
    public TextMeshPro nickNameLable;
    public string photonNickName = "offline";
    public int numberCount = 0;

    public Vector3 mouseInput;
    public Vector3 mousePos;

    // Update is called once per frame
    protected void FixedUpdate()
    {
        this.OwnerController();
        this.LoadOwnerNickName();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView");
        if (stream.IsWriting) this.StreamWriting(stream);
        else this.StreamReading(stream, info);
    }

    protected virtual void StreamWriting(PhotonStream stream)
    {
        stream.SendNext(this.numberCount);
    }

    protected virtual void StreamReading(PhotonStream stream, PhotonMessageInfo info)
    {
        this.numberCount = (int)stream.ReceiveNext();
    }

    protected virtual void OwnerController()
    {

        if (this._photonView.ViewID != 0 && !this._photonView.IsMine) return;

        this.LoadMousePos();
        this.FollowMousePos();
    }

    protected virtual void LoadOwnerNickName()
    {
        this.nickNameLable.text = this.photonNickName+": "+this.numberCount;
        if (this._photonView.ViewID == 0) return;
        this.photonNickName = this._photonView.Owner.NickName;
    }

    protected virtual void LoadMousePos()
    {
        this.mouseInput = Input.mousePosition;
        this.mouseInput.z = Camera.main.nearClipPlane;

        this.mousePos = Camera.main.ScreenToWorldPoint(this.mouseInput);
    }

    protected virtual void FollowMousePos()
    {
        Vector3 xPosLimit = GameManager.instance.xPosLimit;
        Vector3 yPosLimit = GameManager.instance.yPosLimit;
        Vector3 newPos = this.mousePos;
        newPos.z = 0;

        if (newPos.x > xPosLimit.y) newPos.x = xPosLimit.y;
        if (newPos.x < xPosLimit.x) newPos.x = xPosLimit.x;

        if (newPos.y > yPosLimit.y) newPos.y = yPosLimit.y;
        if (newPos.y < yPosLimit.x) newPos.y = yPosLimit.x;

        transform.position = newPos;
    }

}
