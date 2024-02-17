using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class Ice : MonoBehaviourPunCallbacks
{
    public Sprite Ice2;
    public Sprite Ice3;
    public Sprite Ice4;

    SpriteRenderer renderer;

    float iceHP;
    bool playerBlowing;

 

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        iceHP = 1;   //////////////////////////////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        if(iceHP <= 8)
        {
            renderer.sprite = Ice4;
        }
        else if(iceHP <= 16)
        {
            renderer.sprite = Ice3;
        }
        else if (iceHP <= 24)
        {
            renderer.sprite = Ice2;
        }


        if (iceHP <= 0)
        {
            Destroy(gameObject);
            GamePlaying.GetWater();
        }
    }

    [PunRPC]
    void MinusIceHP()
    {
        iceHP--;
    }

   /* void OnTriggerStay2D(Collider2D other)
    {
        if(PlayMission.playerBlowing == true)
        {
        }
    }*/
   
    public void OnWaterButton()
    {
        GameManager.WaterButton.interactable = false;

        StartCoroutine(ClickWaterButton());
    }

   IEnumerator ClickWaterButton()
    {
        GameManager.IsBlow = true;
        yield return new WaitForSeconds(1.0f);
        GameManager.ResetIsBlow();
        photonView.RPC("MinusIceHP", RpcTarget.All);

        GameManager.WaterButton.interactable = true;
    }



}
