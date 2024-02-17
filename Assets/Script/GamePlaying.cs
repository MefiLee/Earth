using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class GamePlaying : MonoBehaviourPunCallbacks, IPunObservable
{
    public static bool AppearanceExit;

    public GameObject AirCrystal;
    public GameObject FireCrystal;
    public GameObject DirtCrystal;
    public GameObject WaterCrystal;
    
    
    public bool HasFire;
    public bool HasAir;
    public bool HasDirt;
    public bool HasWater;

    static bool getFire = false;
    static bool getAir = false;
    static bool getDirt = false;
    static bool getWater = false;



    public GameObject SunShine;
    bool HasSunShine;
    Vector2 RandomAirAreaPoint;

    public GameObject Exit; // 출구 프리팹

    public int ExitArea; // 1~4사이의 난수 지정 >> 출구가 나타날 지역
    public int ExitArea2;

    public static int SunShineScore;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // 동기화 목록 : 원소 유무 상태, 
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HasFire);
            stream.SendNext(HasAir);
            stream.SendNext(HasDirt);
            stream.SendNext(HasWater);

        }
        else
        {
            this.HasFire = (bool)stream.ReceiveNext();
            this.HasAir = (bool)stream.ReceiveNext();
            this.HasDirt = (bool)stream.ReceiveNext();
            this.HasWater = (bool)stream.ReceiveNext();

        }
    }





    void Start()
    {
        AppearanceExit = false;
        HasFire = false;
        HasAir = false;
        HasDirt = false;
        HasWater = false;
        SunShineScore = 0;

        SpawnSunShine();

        ExitArea = Random.Range(1, 5);
        ExitArea2 = Random.Range(1, 5);
        while (ExitArea == ExitArea2) // 첫번째 출구 지역과 두번째 출구 지역이 같으면 두번쨰 출구 지역 다시 설정, 다를 때 까지 반복
        {
            ExitArea2 = Random.Range(1, 5);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.NeedSpawnSunShine == true)
        {
            SpawnSunShine();
            GameManager.NeedSpawnSunShine = false;
        }

        if (getFire == true)
        {
            HasFire = true;
            Vector2 FireCrystalVector = new Vector2(4.4f, 0.44f);
            PhotonNetwork.Instantiate(FireCrystal.name, FireCrystalVector, Quaternion.identity);
            getFire = false;
        }
        if (getAir == true)
        {
            HasAir = true;
            Vector2 AirCrystalVector = new Vector2(0, 4.38f);
            PhotonNetwork.Instantiate(AirCrystal.name, AirCrystalVector, Quaternion.identity);            
            getAir = false;
        }
        if (getDirt == true)
        {
            HasDirt = true;
            Vector2 DirtCrystalVector = new Vector2(0, -4f);
            PhotonNetwork.Instantiate(DirtCrystal.name, DirtCrystalVector, Quaternion.identity);
            getDirt = false;
        }
        if (getWater== true)
        {
            HasWater = true;
            Vector2 WaterCrystalVector = new Vector2(-4.4f, 0.44f);
            PhotonNetwork.Instantiate(WaterCrystal.name, WaterCrystalVector, Quaternion.identity);
            getWater = false;
        }


        if (HasFire == true && HasAir == true && HasDirt == true && HasWater == true) // 정령을 모두 찾았으면 출구 생성 
        {
            GameManager.GolemSpeed = 6f;
            AppearanceExit = true;
            if (PhotonNetwork.IsMasterClient) // 1 : 불지역 , 2: 공기지역, 3: 흙지역 , 4: 물지역                       
            {
                while (ExitArea == ExitArea2) // 첫번째 출구 지역과 두번째 출구 지역이 같으면 두번쨰 출구 지역 다시 설정, 다를 때 까지 반복
                {
                    ExitArea2 = Random.Range(1, 5);
                }

                if (ExitArea == 1 || ExitArea2 == 1)
                {
                    Vector2 FireExitVector = new Vector2(24, 0);
                    PhotonNetwork.Instantiate(Exit.name, FireExitVector, Quaternion.identity);
                }  // 출구지역Index중 둘중 하나라도 1이면 불지역에 출구 생성

                if (ExitArea == 2 || ExitArea2 == 2)
                {
                    Vector2 AirExitVector = new Vector2(0, 30);
                    PhotonNetwork.Instantiate(Exit.name, AirExitVector, Quaternion.identity);
                }  // 출구지역Index 중 둘중 하나라도 2이면 공기지역에 출구 생성

                if (ExitArea == 3 || ExitArea2 == 3)
                {
                    Vector2 DirtExitVector = new Vector2(0, 29);
                    PhotonNetwork.Instantiate(Exit.name, DirtExitVector, Quaternion.identity);
                }  // 출구지역Index 중 둘중 하나라도 3이면 흙지역에 출구 생성

                if (ExitArea == 4 || ExitArea2 == 4)
                {
                    Vector2 WaterExitVector = new Vector2(-22, 0);
                    PhotonNetwork.Instantiate(Exit.name, WaterExitVector, Quaternion.identity);
                }  // 출구지역Index 둘중 하나라도 4이면 물지역에 출구 생성

                ExitArea = 100;
                ExitArea2 = 200;

            }  // 출구 지역 결정 및 출구 생성

        }

    }


    public void SpawnSunShine()
    {
        RandomAirAreaPoint = new Vector2(Random.Range(-10, 10), Random.Range(13, 33));
        Instantiate(SunShine, RandomAirAreaPoint, Quaternion.identity);
    }

    


    //------------------------------------------------------- 원소 얻기 함수. 각 항목별로 획득 사운드 추가 ( CM7 )
    public static void GetFire()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            getFire = true;
        }

    }   // 전역함수 >> getElement = true >> if(getElement) >> HasElement >> 동기화

    public static void GetAir()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            getAir = true;
        }
    }

    public static void GetDirt()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            getDirt = true;
        }
    }

    public static void GetWater()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            getWater = true;
        }
    }


   
   
}
