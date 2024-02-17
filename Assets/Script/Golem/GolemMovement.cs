using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GolemMovement : MonoBehaviourPun
{
    public float moveSpeed;
    public AudioClip walk;
    public AudioClip attack;

    bool moveAble;
    PhotonView myPV;


    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody2D playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private SpriteRenderer rend;
    private AudioSource audio;

    public int MalconCount;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        myPV = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();
        moveAble = false;

        audio.clip = walk;
        if (photonView.IsMine)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = 7f;
        }

        MalconCount = PhotonNetwork.CurrentRoom.PlayerCount-1;

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.GolemSpeed != 0)
        {
            moveSpeed = GameManager.GolemSpeed;
        }
        if (!photonView.IsMine)
        {
            return;
        }

        if(GameManager.attackCode == false)
        {
            playerAnimator.SetBool("IsAttack", false);

            if (moveAble)
            {
                
                Move();
            }
        }
        else
        {
            playerRigidbody.velocity = new Vector3(0, 0, 0) * 0f;
            audio.clip = attack;
            if (!audio.isPlaying)
            {

                audio.Play();
            }
            playerAnimator.SetBool("IsAttack", true);
            
        }
        
        if(GamePlaying.AppearanceExit == true)
        {
            moveSpeed = 5f;
        }


        

        if(GameManager.KillMalcon >=  MalconCount)
        {
            GameManager.GolemEnd = 1;

        }else if(GameManager.KillMalcon + GameManager.ExitMalcon >= MalconCount)
        {
            GameManager.GolemEnd = 2;
        }






    }


    void OnEnable()
    {
        Invoke("StartMoving", 5f);
    }

    

    void StartMoving()
    {
        moveAble = true;
        playerAnimator.SetBool("IsGameStart", true);
    }


    private void Move()
    {

        bool isMove = false;

        Vector3 vector = new Vector3(playerInput.Xmove, playerInput.Ymove, 0);
        playerRigidbody.velocity = vector * moveSpeed;

        if (Input.GetAxis("Horizontal") < 0)
        {
            //rend.flipX = true;
            myPV.RPC("Flip", RpcTarget.All, true);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            //rend.flipX = false;
            myPV.RPC("Flip", RpcTarget.All, false);

        }

        

        if (playerInput.Xmove != 0 || playerInput.Ymove != 0)
        {
            isMove = true;
            if (!audio.isPlaying)
            {
                audio.clip = walk;
                audio.Play();
            }
        }
        else
        {
            if(audio.clip == walk)
            {
                audio.Stop();

            }
        }

        playerAnimator.SetBool("IsMove", isMove);
    }


    [PunRPC]
    bool Flip(bool f)
    {
        return rend.flipX = f;
    }




   


}
