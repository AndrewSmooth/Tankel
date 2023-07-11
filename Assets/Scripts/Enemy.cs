using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

   
    public float speed;
    private Transform player;

    private GameObject _public_Var;
    public GameObject shell_abstract;
    private GameObject tank_tower;
    private GameObject tank_fictive_gun;
    private GameObject tank_gun;
    
   

    private float shot_power = 40f;
    private float shot_damage = 25f;
    private float max_health = 100f;
    public float current_health;
    //public AudioClip shot_clip;
    public bool shot_allowed = true;
    private float shot_cooldown_count = 0f;

    void Start()
    {
        current_health = max_health;
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _public_Var = GameObject.Find("___public_variables_class");

        tank_tower = transform.GetChild(0).gameObject;
        tank_fictive_gun = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        tank_gun = tank_fictive_gun.transform.GetChild(0).gameObject;
        tank_fictive_gun.transform.Rotate(-10f, 0, 0, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        if (current_health <= 0)
        {
            Destroy(gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, player.position, 100 * Time.deltaTime, 100f));
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        Shot_Check();

    }

    


    private void Shot_Check()
    {
         
        shot_cooldown_count += Time.deltaTime;
        if (shot_cooldown_count >= _public_Var.GetComponent<Public_Variables_Class>().shot_cooldown)
        {
            shot_allowed = true;
        }
        if (shot_allowed)
        {
            Shoot();
            shot_allowed = false;
            shot_cooldown_count = 0;
        }

    }

    private void Shoot()
    {
        GameObject shell = Instantiate(shell_abstract) as GameObject;
        shell.transform.position = tank_gun.transform.position;
        shell.transform.rotation = tank_gun.transform.rotation;
        shell.transform.Translate(0, 0, 1.5f, Space.Self);
        shell.GetComponent<Rigidbody>().AddRelativeForce(shell.transform.forward * shot_power, ForceMode.VelocityChange);
        //Play_Audio(shot_clip);

    }

}
