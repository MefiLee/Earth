using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class ButtonManager : MonoBehaviourPunCallbacks, IPunObservable
{
    
    public Button startButton;
    public bool IsPlayGame;
    public static ButtonManager B_instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (Bm_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                Bm_instance = FindObjectOfType<ButtonManager>();
            }

            // 싱글톤 오브젝트를 반환
            return Bm_instance;
        }
    }
    private static ButtonManager Bm_instance; // 싱글톤이 할당될 static 변수


private void Awake()
    {
        if (B_instance != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name != "WaitingRoom")
        {
            gameObject.SetActive(false);
        }
        startButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
      



        try
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 1 && PhotonNetwork.IsMasterClient)
            {
                startButton.interactable = true;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("NullReferenceException");
        }

        try
        {
            if (IsPlayGame == true)
            {

                PhotonNetwork.LoadLevel("Main");
            }
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("오류 멈춰!");
        }

    }

    [PunRPC]
    public void GameStart()
    {
        IsPlayGame = true;
    }


    public void OnGameStartButton()
    {
        photonView.RPC("GameStart", RpcTarget.All);
    } // 시작버튼 (메인씬 로드)
   
    public void OnCloseButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() // 룸을 나갈 때 자동으로 실행
    {
        // 룸을 나가면 로비 씬으로 돌아감
        SceneManager.LoadScene("Lobby");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    public void ActiveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }
    
    

    

}
