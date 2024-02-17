using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class ChooseGolem : MonoBehaviourPunCallbacks, IPunObservable
{


    public float WaitingSec;

    public GameObject Malcon;
    public GameObject Golem;
    public int GolemIndex = 200;
    PhotonView myPV;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GolemIndex);
        }
        else
        {
            this.GolemIndex = (int)stream.ReceiveNext();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            GolemIndex = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount-1);

        }


        Invoke("Summon", WaitingSec);
    }

    

    

    void Summon()
    {
        Vector3 SpawnPosition = new Vector3(0, 0, 0); // 스폰포인트 추가 요망


        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Golem.name, SpawnPosition, Quaternion.identity);
            GameManager.instance.isGolem = true;
        }
        else
        {
            PhotonNetwork.Instantiate(Malcon.name, SpawnPosition, Quaternion.identity);
            GameManager.instance.isGolem = false;
        }
        


    }



}
