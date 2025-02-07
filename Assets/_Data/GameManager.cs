using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Vector2 xPosLimit = new Vector2(-14, 14);
    public Vector2 yPosLimit = new Vector2(-6, 6);

    public int maxNumber = 100;
    public int numberPerLine = 15;
    public string numberPrefab = "PhotonNumber";
    public string numberLimitPrefab = "PhotonNumberLimit";
    public List<int> numbers = new List<int>();
    public List<PhotonNumber> photonNumbers = new List<PhotonNumber>();

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

        if (PhotonNetwork.IsMasterClient) this.MasterSpawnNumbers();
        else this.ClientReceivedNumbers();
    }

    protected virtual void ClientReceivedNumbers()
    {
        Debug.Log("ClientReceivedNumbers");
        PhotonNumber[] photonNumbers = GameObject.FindObjectsOfType<PhotonNumber>();
        foreach (PhotonNumber photonNumber in photonNumbers)
        {
            this.photonNumbers.Add(photonNumber);
        }

        if (this.photonNumbers.Count == 0) Invoke("ClientReceivedNumbers", 1f);
    }

    protected virtual void MasterSpawnNumbers()
    {
        Debug.Log("MasterSpawnNumbers");

        for (int i = 0; i < this.maxNumber; i++)
        {
            int lineNumber = Mathf.RoundToInt(i / this.numberPerLine);
            int colNumber = Mathf.RoundToInt(i % this.numberPerLine);

            this.SpawnNumber(i, lineNumber, colNumber);
        }

        this.SpawnNumberLimit();
    }

    protected virtual void SpawnNumberLimit()
    {
        GameObject numberObj = PhotonNetwork.Instantiate(this.numberLimitPrefab, Vector3.zero, Quaternion.identity);
        numberObj.transform.position = this.EndPoint();
    }

    protected virtual void SpawnNumber(int number, int lineNumber, int colNumber)
    {

        GameObject numberObj;
        numberObj = this.SpawnNumberNetwork(number);

        Vector3 pos = this.StartPoint();
        pos.x += colNumber * 3.5f;
        pos.y -= lineNumber * 4f;

        numberObj.transform.position = pos;
        PhotonNumber photonNumber = numberObj.GetComponent<PhotonNumber>();
        photonNumber.Set(this.GetNumber());

        this.photonNumbers.Add(photonNumber);
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

    protected virtual GameObject SpawnNumberNetwork(int i)
    {
        return PhotonNetwork.Instantiate(this.numberPrefab, Vector3.zero, Quaternion.identity);
    }

    protected virtual Vector3 StartPoint()
    {
        return new Vector3(this.xPosLimit.x, this.yPosLimit.y, 0);
    }

    protected virtual Vector3 EndPoint()
    {
        return new Vector3(this.xPosLimit.y, this.yPosLimit.x, 0);
    }

    public virtual void NumberOnClaimed(int number)
    {
        PhotonNumber photonNumber = this.FindPhotonNumber(number);
        if (!photonNumber)
        {
            Debug.LogWarning("Cant find number: " + number.ToString());
            return;
        }

        photonNumber.Claimed();

        if (PhotonNetwork.IsMasterClient) PhotonNumberLimit.instance.Set(photonNumber.number + 1);
    }

    public virtual PhotonNumber FindPhotonNumber(int number)
    {
        foreach (PhotonNumber photonNumber in this.photonNumbers)
        {
            if (photonNumber.number == number) return photonNumber;
        }

        return null;
    }
}
