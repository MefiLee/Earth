using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        

    }



    // Update is called once per frame
    void Update()
    {
       if(GameManager.InBush ==true)
        {
            SetBush();
        }
        else
        {
            ResetBush();
        }
    }


    

    public  void SetBush()
    {

        renderer.color = new Color(1, 1, 1, 0.4f);

    }

    public void ResetBush()
    {

        renderer.color = new Color(1, 1, 1, 1);
    }
    
}
