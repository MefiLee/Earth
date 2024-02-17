using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MalconMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public GameObject DeadMalcon;
    public GameObject GhostMalcon;

    private SpriteRenderer rend;
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody2D playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private AudioSource audio;
    PhotonView myPV;
    Transform trans;

    public bool isDead;

    public AudioClip walk;
    public AudioClip blow;


    void Awake()
    {
        isDead = false;
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        myPV = GetComponent<PhotonView>();
        trans = GetComponent<Transform>();
        audio = GetComponent<AudioSource>();

    }


    void Start()
    {
        audio.clip = walk;
        isDead = false;
        
        if (photonView.IsMine)
        {

            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = 5f;
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Timer.time <= 0)
        {
            Die();
        }

            if (isDead == false)
        {
            Move();
            if (GameManager.IsBlow == true)
            {
                playerRigidbody.velocity = new Vector3(0, 0, 0) * 0f;
                playerAnimator.SetBool("IsBlow", GameManager.IsBlow);
            }
            else if(GameManager.IsBlow == false)
            {
                playerAnimator.SetBool("IsBlow", GameManager.IsBlow);
            }
        }
        else if (isDead == true)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(null);
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate(GhostMalcon.name, trans.position, Quaternion.identity);
            }
            PhotonNetwork.Instantiate(DeadMalcon.name, trans.position, Quaternion.identity);
            myPV.RPC("AddKillMalcon", RpcTarget.All);
            PhotonNetwork.Destroy(gameObject);
            GameManager.EndGame();

        }




        if (GameManager.isGameWin == true)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(null);

            gameObject.SetActive(false);
            PhotonNetwork.Destroy(gameObject);
        }
    }



    private void FixedUpdate()
    {
        // 로컬 플레이어만 직접 위치와 회전을 변경 가능


        // 입력값에 따라 애니메이터의 Move 파라미터 값을 변경
        // playerAnimator.SetFloat("Move", playerInput.Xmove);
        // playerAnimator.SetFloat("Move", playerInput.Zmove);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
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
            
            playerAnimator.SetBool("IsMove", isMove);

            if (!audio.isPlaying)
            {
                audio.clip = walk;
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
        }


        playerAnimator.SetBool("IsMove", isMove);
    }


    [PunRPC]
    bool Flip(bool f)
    {
        return rend.flipX = f;
    }





    void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Golem" && GameManager.attackCode == true)
        {
                Die();
        }


    }

    

    public void Die()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            isDead = true;
        }
    }

    [PunRPC]
    public void AddKillMalcon()
    {
        GameManager.KillMalcon++;
    }

}







