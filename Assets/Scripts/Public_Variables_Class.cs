using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Public_Variables_Class : MonoBehaviour
{
    public bool TurnLeft = false;
    public bool TurnUp = false;
    public bool TurnRight = false;
    public bool TurnDown = false;

    public float tower_angle_horizontal;
    public float tower_angle_vertical;
    public float shot_cooldown = 1.5f;
    public bool game_over = false;

    private float scores_to_win = 6;
    public float scores = 0;

    void Update()
    {
     if (scores >= scores_to_win)
        {
            print("Игра окончена");
            game_over = true;
            
        }   
    }
}
