using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public static float time;
    public float sec;
    public Text timerText;
    void Start()
    {
        time = 306f;
        StartCoroutine("StopWatch");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StopWatch()
    {
        while(true){
            time -= Time.deltaTime;
            sec = (int)(time );
            timerText.text = sec.ToString();
            yield return null;

            if(time <= 0)
            {
                GameManager.GolemEnd = 1;
                break;
            }
        }
            
        
    }
}

