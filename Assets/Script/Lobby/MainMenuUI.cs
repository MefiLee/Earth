using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine.UI;
using System;

public class MainMenuUI : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    public InputField nameInputField;
    public GameObject inputyourname;

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings(); // 마스터서버 접속

    }

    public void OnClickMakeRoomButton()//방만들기 버튼
    {
        if (nameInputField.text != "")
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8 });
        }
        else
        {
            inputyourname.SetActive(true);
        }



    }
    
    public void OnClickFindRoomButton()
    {
        if (nameInputField.text != "")
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();

            }
            else
            {
                Debug.Log("Can't Connected Server");
            }
        }
        else
        {
            inputyourname.SetActive(true);
        }
    }


        
       
     //방찾기 버튼
    public void OnClickInPutButton()
    {
        Debug.Log("Input your Code!");
    } //코드입력버튼



    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    } // 종료버튼 

    public override void OnDisconnected(DisconnectCause cause) // 서버 접속 실패시 실행
    {
        Debug.Log("Connect False : Reconnecting");
       
        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // 빈 방이 없어서 룸 참가에 실패했을 경우
    {
        Debug.Log("No Empty Room. Makeing Room..");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 6 });
        
    }
    public override void OnJoinedRoom()
    {
        // 모든 룸 참가자들이 Main 씬을 로드하게 함
        PhotonNetwork.LoadLevel("WaitingRoom");
    } // 룸에 참가 완료된 경우 자동 실행

}
