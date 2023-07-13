using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    
    private GameObject enemy1;
    private GameObject enemy2;
    private GameObject enemy3;
    private GameObject enemy4;

    public float reset1_count = 0;
    public float reset2_count = 0;
    public float reset3_count = 0;
    public float reset4_count = 0;
    public float reset_time = 20f;

    public GameObject enemy_abstract;
    private Vector3 start_enemy1_position = new Vector3(50, 0, 20);
    private Vector3 start_enemy2_position = new Vector3(-30, 0, 20);
    private Vector3 start_enemy3_position = new Vector3(10, 0, 57);
    private Vector3 start_enemy4_position = new Vector3(11, 0, -16);


    void Start()

    {   
        enemy1 = Instantiate(enemy_abstract) as GameObject;
        enemy1.transform.position = start_enemy1_position;

        enemy2 = Instantiate(enemy_abstract) as GameObject;
        enemy2.transform.position = start_enemy2_position;

        enemy3 = Instantiate(enemy_abstract) as GameObject;
        enemy3.transform.position = start_enemy3_position;

        enemy4 = Instantiate(enemy_abstract) as GameObject;
        enemy4.transform.position = start_enemy4_position;

    }

    void Update() 
    {
        
        if (enemy1.IsDestroyed())
        {
            reset1_count += Time.deltaTime;
            if (reset1_count >= 10)
            {
                enemy1 = Instantiate(enemy_abstract) as GameObject;
                enemy1.transform.position = start_enemy1_position;
                reset1_count = 0;
            }
        }
        if (enemy2.IsDestroyed())
        {
            reset2_count += Time.deltaTime;
            if (reset2_count >= 10)
            {
                enemy2 = Instantiate(enemy_abstract) as GameObject;
                enemy2.transform.position = start_enemy2_position;
                reset2_count = 0;
            }
        }
        if (enemy3.IsDestroyed())
        {
            reset3_count += Time.deltaTime;
            if (reset3_count >= 10)
            {
                enemy3 = Instantiate(enemy_abstract) as GameObject;
                enemy3.transform.position = start_enemy3_position;
                reset3_count = 0;
            }
        }
        if (enemy4.IsDestroyed())
        {
            reset4_count += Time.deltaTime;
            if (reset4_count >= 10)
            {
                enemy4 = Instantiate(enemy_abstract) as GameObject;
                enemy4.transform.position = start_enemy4_position;
                reset4_count = 0;
            }
        }

    }
}
