using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScrolling : MonoBehaviour
{
    public int speed = 5;

    Vector3 vector = new Vector3(1,0,0);

    void Update()
    {
        transform.Translate(vector * speed * Time.deltaTime );
        

        if(transform.position.x >= 15)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(-30, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
