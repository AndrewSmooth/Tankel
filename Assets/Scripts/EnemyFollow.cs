using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    
    private GameObject enemy;

    public GameObject enemy_abstract;
    private Vector3 start_enemy_position = new Vector3(30f, 1f, 30f);


    void Start()

    {
        enemy = Instantiate(enemy_abstract) as GameObject;
        enemy.transform.position = start_enemy_position;
        
    }

    void Update() 
    {
        
    }
}
