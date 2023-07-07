using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Script : MonoBehaviour
{
    private float time_count = 0;
    public AudioClip clip;
    void Start()
    {
        Play_Audio(clip);
    }

    void Update()
    {
        time_count += Time.deltaTime;
        if (time_count >= 2f)
        {
            Destroy(gameObject);
        }
    }

    private void Play_Audio(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}

