using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingCloud : MonoBehaviour
{
    public int speed;
   
    void start()
    {
        speed = Random.Range(1, 4);
    }


    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if(transform.position.x <= -15)
        {
            Reposition();
        }

    }
    private void Reposition()
    {
        speed = Random.Range(1, 4);
        Vector2 offset = new Vector2(30, 0);
        transform.position = (Vector2)transform.position + offset;
    }

}
