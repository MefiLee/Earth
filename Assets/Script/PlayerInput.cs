using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerInput : MonoBehaviourPun
{
    public string XmoveAxisName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string YmoveAxisName = "Vertical"; // 위아래움직임을 위한 입력축 이름

    public float Xmove { get; private set; } // 감지된 움직임 입력값
    public float Ymove { get; private set; } // 감지된 움직임 입력값



    // Update is called once per frame
    void Update()
    {
        // 로컬 플레이어가 아닌 경우 입력을 받지 않음
        if (!photonView.IsMine)
        {
            return;
        }

        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        //if (GameManager.instance != null && GameManager.isGameover)
        //{
        //    Xmove = 0;
        //    Ymove = 0;
        //    return;
        //}

        Xmove = Input.GetAxis(XmoveAxisName);
        Ymove = Input.GetAxis(YmoveAxisName);
    }
}
