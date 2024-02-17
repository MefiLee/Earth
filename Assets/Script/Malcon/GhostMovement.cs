using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GhostMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f; 

    private SpriteRenderer rend;
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody2D playerRigidbody; // 플레이어 캐릭터의 리지드바디
    PhotonView myPV;
    Transform trans;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        myPV = GetComponent<PhotonView>();
        trans = GetComponent<Transform>();

        if (photonView.IsMine)
        {

            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = 5f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            Move();
        }

    }

    private void Move()
    {

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


        


    }

    [PunRPC]
    bool Flip(bool f)
    {
        try
        {
            return rend.flipX = f;

        }
        catch(NullReferenceException e)
        {
            Debug.Log("NullException");

            return rend.flipX = f;
        }
    }
}
