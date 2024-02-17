using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class PlayMission : MonoBehaviourPunCallbacks
{


    PhotonView myPV;

    public bool HasSunShine;

    private AudioSource audio;


    public static int PlayerDigCode;
    public static bool playerBlowing;


    public AudioClip blow;
    public AudioClip GetSunshine;
    public AudioClip GiveSunshine;
    public AudioClip breakSoil;


    void Start()
    {
        myPV = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();
        HasSunShine = false;
    }


    void Update()
    {
        PlayerDigCode = 0;
        playerBlowing = false;
    }





    void OnDisable()
    {
        //GameManager.ExitMalcon++; ( 포톤네트워크 RPC 사용해서 전체 숫자 동기화하기 )
        // GameManager.GameOverText.SetActive(true);
        // 카메라 이동 설정
    }



    


    void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (myPV.IsMine && other.tag == "FireTree" && other.gameObject.GetComponent<FireTree>().isFire == true)
            {
                GameManager.FireButton.interactable = true;
            }
            if (myPV.IsMine && other.tag == "Bush")  // NullReferenceException
            {
                GameManager.InBush = true;
            } 

            

            if (myPV.IsMine && other.tag == "SunShine")
            {
                HasSunShine = true;
                audio.clip = GetSunshine;
                audio.Play();
                Destroy(other.gameObject);
            } // 햇살 먹기

            if(myPV.IsMine&&other.tag == "Exit")
            {
                GameManager.isGameWin = true;
                myPV.RPC("AddExitMalcon", RpcTarget.All);
            }

            if (myPV.IsMine && other.tag == "Ice")
            {
                GameManager.WaterButton.interactable = true;
            }

        }catch(NullReferenceException e)
        {
            Debug.Log("NullException");
        }
        




        if (other.tag == "Soil")
        {
            audio.clip = breakSoil;
            audio.Play();
            GameManager.DestroyedSoil++;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (myPV.IsMine && other.tag == "Bush")
        {
            GameManager.InBush = false;
        }


        if (myPV.IsMine && other.tag == "Ice")
        {
            GameManager.WaterButton.interactable = false;
        }

        if (myPV.IsMine && other.tag == "FireTree")
        {
            GameManager.FireButton.interactable = false;
        }


    }

    void OnTriggerStay2D(Collider2D other)
    {
        


        if (myPV.IsMine && other.tag == "AirTree")
        {
            if (HasSunShine)
            {
                myPV.RPC("AddSunShine", RpcTarget.All);
                audio.clip = GiveSunshine;
                audio.Play();
                GameManager.NeedSpawnSunShine = true;
                HasSunShine = false;
            }
        } // 햇살 나무에 바치기


    }


    [PunRPC]
    public void AddSunShine()
    {
        GamePlaying.SunShineScore++;

        if (GamePlaying.SunShineScore >= 1)//////////////////////////////////////////////
        {

            GamePlaying.GetAir();
        }
    }


    [PunRPC]
    public void AddExitMalcon()
    {
        GameManager.ExitMalcon++;
    }

}
