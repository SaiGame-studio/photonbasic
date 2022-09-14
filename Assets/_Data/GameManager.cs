using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Vector2 xPosLimit = new Vector2(-14, 14);
    public Vector2 yPosLimit = new Vector2(-6, 6);

    public int maxNumber = 100;
    public int numberPerLine = 15;
    public string numberPrefab = "PhotonNumber";
    public List<int> numbers = new List<int>();

    protected void Awake()
    {
        GameManager.instance = this;
    }

    protected void Start()
    {
        this.RandomNumber();
        this.GameStart();
    }

    public virtual void GameStart()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Invoke("GameStart", 1f);
            return;
        }

        for (int i = 0; i < this.maxNumber; i++)
        {
            int lineNumber = Mathf.RoundToInt(i / this.numberPerLine);
            int colNumber = Mathf.RoundToInt(i % this.numberPerLine);

            this.SpawnNumber(i, lineNumber, colNumber);
        }
    }

    protected virtual void SpawnNumber(int number, int lineNumber, int colNumber)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject numberObj;
        //if (PhotonNetwork.NetworkClientState == ClientState.Joined) numberObj = this.SpawnNetwork(number);
        //else numberObj = this.SpawnLocal(number);

        numberObj = this.SpawnNetwork(number);

        Vector3 pos = this.StartPoint();
        pos.x += colNumber*3.5f;
        pos.y -= lineNumber*4f;

        numberObj.transform.position = pos;
        PhotonNumber photonNumber = numberObj.GetComponent<PhotonNumber>();
        photonNumber.Set(this.GetNumber());
    }

    protected virtual void RandomNumber()
    {
        for (int i = 0; i < this.maxNumber; i++)
        {
            this.numbers.Add(i);
        }
    }

    protected virtual int GetNumber()
    {
        int rand = Random.Range(0, this.numbers.Count);
        int number = this.numbers[rand];
        this.numbers.RemoveAt(rand);
        return number;
    }

    protected virtual GameObject SpawnLocal(int i)
    {
        GameObject numberLocal = Resources.Load(this.numberPrefab) as GameObject;
        return Instantiate(numberLocal);
    }

    protected virtual GameObject SpawnNetwork(int i)
    {
        return PhotonNetwork.Instantiate(this.numberPrefab, Vector3.zero, Quaternion.identity);
    }

    protected virtual Vector3 StartPoint()
    {
        return new Vector3(this.xPosLimit.x, this.yPosLimit.y, 0);
    }
}
