using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviour
{
    public InputField nameInputField;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            return;
        }
        else
        {
            string PlayerName = PlayerPrefs.GetString("PlayerName");
            nameInputField.text = PlayerName;
        }
    }

  
    public void PlacePlayerName()
    {
        string PlayerNickName = nameInputField.text;
        PhotonNetwork.NickName = PlayerNickName;
        PlayerPrefs.SetString("PlayerName",PlayerNickName);
    }

   
}
