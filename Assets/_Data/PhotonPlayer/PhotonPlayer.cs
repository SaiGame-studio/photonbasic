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
    public Vector2 xPosLimit = new Vector2(-10, 10);
    public Vector2 yPosLimit = new Vector2(-5, 6);

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
        Vector3 newPos = this.mousePos;
        newPos.z = 0;

        if (newPos.x > this.xPosLimit.y) newPos.x = this.xPosLimit.y;
        if (newPos.x < this.xPosLimit.x) newPos.x = this.xPosLimit.x;

        if (newPos.y > this.yPosLimit.y) newPos.y = this.yPosLimit.y;
        if (newPos.y < this.yPosLimit.x) newPos.y = this.yPosLimit.x;

        transform.position = newPos;
    }
}
