using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class FireTree : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool isFire;
    Animator animator;
    PhotonView myPV;
    private AudioSource audio;
    public int FireCode;

    int FireNum;

    public Button firebutton;



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // isFire 동기화
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isFire);
        }
        else
        {
            this.isFire = (bool)stream.ReceiveNext();
        }

    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        myPV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        isFire = false;
    }

    void Update() // isfire대로 애니메이터 실행
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.FireNum == FireCode)
            {
                myPV.RPC("SetFire", RpcTarget.All);
            }
        }

        animator.SetBool("IsFire", isFire);

        if(isFire == true)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
        }






    }
    [PunRPC]
    void SetFire()
    {
        isFire = true;
    }




    public void OnFireButton()
    {
        myPV.RPC("BlowFire", RpcTarget.All);
        GameManager.FireButton.interactable = false;
        GameManager.IsBlow = true;
        Invoke("ResetBlow", 1f);
    }

    [PunRPC]
    void BlowFire()
    {
        // 플레이어의 부는 애니메이션 재생

        if (isFire == true)
        {
            isFire = false;
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.FireNum = 0;
                if (Random.Range(1, 2) == 1)///////////////////////////////////////////
                {
                    GamePlaying.GetFire();
                }
            }
        }

    }

    void ResetBlow()
    {
        GameManager.ResetIsBlow();
    }

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (isFire && other.tag == "Malcon")
    //    {
    //        GameManager.syncFire = true;
    //    }
    //}

    

}







