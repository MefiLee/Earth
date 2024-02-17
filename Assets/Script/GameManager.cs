using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public bool isGolem;

    public static int ExitMalcon;
    public static int KillMalcon;

    public static int FireNum = 0;
    public static int DestroyedSoil;

    public static Button FireButton;         // 다른 스크립트에서 사용할 변수
    public static Button DirtButton;
    public static Button WaterButton;

    public Button firebutton;
    public Button waterbutton;
    public Button attackbutton;
    public Button runbutton;
    public Text GameOverText;
    public Text GameWinText;
    
    public Text GolemGameWinText;
    public Text GolemGameOverText;

    public Text ExitMalconNumber;
    public Text KillMalconNumber;

    public static int GolemEnd;

    public static bool InBush;
    public static bool NeedSpawnSunShine;

    public static bool attackCode;
    PhotonView myPV;

    public static bool isGameWin;

    public static bool IsBlow;

    public static float GolemSpeed;

    public static GameManager instance // 외부에서 싱글톤을 가져올 때 사용할 변수
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수
    public static bool isGameover; //{ get; private set; }  게임 오버 상태


    private void Awake()
    {
        attackCode = false;
        isGameWin = false;
        isGameover = false;
        GolemSpeed = 3f;
        DestroyedSoil = 0;
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            Destroy(gameObject);  // 자신을 파괴
        }
        WaterButton = waterbutton;
        FireButton = firebutton;
    }

    private void Start()
    {
        IsBlow = false;
        myPV = GetComponent<PhotonView>();
        Invoke("SetButton", 5);
        if (PhotonNetwork.IsMasterClient)
        {

            FireNum = Random.Range(1, 17);
        }
        InBush = false;
        NeedSpawnSunShine = false;
        ExitMalcon = 0;
        KillMalcon = 0;
        isGameover = false;
        GolemEnd = 0;
        GameOverText.gameObject.SetActive(false);
        GameWinText.gameObject.SetActive(false);
        GolemGameOverText.gameObject.SetActive(false);
        GolemGameWinText.gameObject.SetActive(false);

    }

    // 게임 오버 처리
    public static void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
    }

    // 키보드 입력을 감지하고 룸을 나가게 함
    private void Update()
    {
        if(isGameover == true)
        {
            GameOverText.gameObject.SetActive(true);
        }else if(isGameWin == true)
        {
            GameWinText.gameObject.SetActive(true);

        }
        if (PhotonNetwork.IsMasterClient && FireNum == 0)
        {

            FireNum = Random.Range(1, 17);
        }

        if(DestroyedSoil == 10)
        {
            GamePlaying.GetDirt();
            DestroyedSoil++;
        }

        if(isGolem == true)
        {
            if (GolemEnd == 1)
            {
                SetGolemWin();
            }
            else if (GolemEnd == 2)
            {
                SetGolemGameOver();
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }


    }

    // 룸을 나갈때 자동 실행되는 메서드
    public override void OnLeftRoom()
    {
        // 룸을 나가면 로비 씬으로 돌아감
        SceneManager.LoadScene("Lobby");
    }

    void SetButton()
    {
        if (isGolem == true)
        {
            attackbutton.gameObject.SetActive(true);
            runbutton.gameObject.SetActive(true);

            attackbutton.interactable = false;
            runbutton.interactable = false;


            Invoke("ResetAttackCode", 10f);
            Invoke("ResetRunButton", 10f);
        }
        else if (isGolem == false)
        {
            firebutton.gameObject.SetActive(true);
            waterbutton.gameObject.SetActive(true);
        }
    }

    public void OnAttackButton()
    {
        myPV.RPC("SetAttackCode", RpcTarget.All);
        attackbutton.interactable = false;
    }

    public void OnRunButton()
    {
        runbutton.interactable = false;
        GolemSpeed = 5.5f;
        Invoke("ResetMoveSpeed", 2f);
    }

    public void ResetMoveSpeed()
    {
        GolemSpeed = 3f;
        Invoke("ResetRunButton", 30f);
    }

    public void ResetRunButton()
    {
        runbutton.interactable = true;
    }
    
   [PunRPC]
   void SetAttackCode()
    {
        attackCode = true;
        Invoke("ResetAttackCode", 1);

    }

    void ResetAttackCode()
    {
        attackCode = false;
        attackbutton.interactable = true;
    }

    public void SetGolemWin()
    {
        GolemGameWinText.gameObject.SetActive(true);
    }

    public void SetGolemGameOver()
    {
        ExitMalconNumber.text = "Exit Malcon : "+ ExitMalcon;
        KillMalconNumber.text = "Kill Malcon : "+ KillMalcon;
        GolemGameOverText.gameObject.SetActive(true);
    }

    public static void ResetIsBlow()
    {
        IsBlow = false;
    }
}
