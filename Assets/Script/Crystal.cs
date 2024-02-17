using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public SpriteRenderer renderer;
    public AudioClip SpawnCrystalSound;
    private AudioSource audio;
    float alpha;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        alpha = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        




    }

    void OnEnable()
    {
        audio = GetComponent<AudioSource>();
        audio.clip = SpawnCrystalSound;
        audio.Play();
        StartCoroutine("SetAlpha");

    }

   
    IEnumerator SetAlpha()
    {
        for(int i = 0; i < 100; i++)
        {
            renderer.color = new Color(1, 1, 1, alpha);
            alpha += 0.01f;

            yield return new WaitForSeconds(0.01f);

        }

    }


}
