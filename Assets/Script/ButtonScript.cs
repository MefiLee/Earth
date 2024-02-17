using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public AudioSource bop;
    


    public void OnClickButton()
    {
        bop.Play();
    }
}
