using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // 게임 플레이어 프리팹
    public GameObject GolemPrefab; // 게임 플레이어 프리팹



    public static WaitingRoomManager W_instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (Wm_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                Wm_instance = FindObjectOfType<WaitingRoomManager>();
            }

            // 싱글톤 오브젝트를 반환
            return Wm_instance;
        }
    }
    private static WaitingRoomManager Wm_instance; // 싱글톤이 할당될 static 변수

    private void Awake()
    {
        if (W_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Vector3 SpawnPosition = new Vector3(0, -1, 0);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(GolemPrefab.name, SpawnPosition, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, SpawnPosition, Quaternion.identity);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    

}
