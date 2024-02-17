using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class WaitingRoomUIManager : MonoBehaviourPunCallbacks
{
    public static WaitingRoomUIManager U_instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (Um_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                Um_instance = FindObjectOfType<WaitingRoomUIManager>();
            }

            // 싱글톤 오브젝트를 반환
            return Um_instance;
        }
    }
    private static WaitingRoomUIManager Um_instance; // 싱글톤이 할당될 static 변수


    private void Awake()
    {
        if (U_instance != this)
        {
            Destroy(gameObject);
        }
    }
    public Text PlayerNumbersText;
    int number;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ButtonManager.B_instance.ActiveStartButton();
        }
    }

    // Update is called once per frame
    void Update()
    {

        try
        {

            PlayerNumbersText.text = "Player : ( " + PhotonNetwork.CurrentRoom.PlayerCount + " / 6)";
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("NullReferenceException");
        }
        
    }



    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ButtonManager.B_instance.ActiveStartButton();
        }
    }
}
