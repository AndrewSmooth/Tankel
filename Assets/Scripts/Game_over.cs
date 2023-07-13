using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_over : MonoBehaviour
{
    public AudioClip defeat_clip;
    public AudioClip game_over_clip;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "DeathScene")
            Play_Audio(defeat_clip);
        if (SceneManager.GetActiveScene().name == "GameOverScene")
            Play_Audio(game_over_clip);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void Play_Audio(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
