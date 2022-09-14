using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{

    public PhotonView photonView;
    public TextMeshPro nickNameLable;
    public string photonNickName = "offline";

    public Vector3 mouseInput;
    public Vector3 mousePos;

    // Update is called once per frame
    protected void FixedUpdate()
    {
        this.OwnerController();
        this.LoadOwnerNickName();
    }

    protected virtual void OwnerController()
    {

        if (this.photonView.ViewID != 0 && !this.photonView.IsMine) return;

        this.LoadMousePos();
        this.FollowMousePos();
    }

    protected virtual void LoadOwnerNickName()
    {
        this.nickNameLable.text = this.photonNickName;
        if (this.photonView.ViewID == 0) return;
        this.photonNickName = this.photonView.Owner.NickName;
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
