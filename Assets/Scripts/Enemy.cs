using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{

    
    public float speed;
    private Transform player;
    public AudioClip shot_clip;
    private GameObject _public_Var;
    public GameObject shell_abstract;

    public Canvas canvas;
    private Canvas_Bar canvas_bar;
          
    private float distance;
    private bool tower_rotation = true;
    public float tower_rotationi_speed = 50;
    public float tank_rotation_speed = 100;

    private GameObject tank_tower;
    private GameObject tank_fictive_gun;
    private GameObject tank_gun;
    
    private float shot_power = 40f;
    private float shot_damage = 25f;    
    private float max_health = 100f;
    public float current_health;
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
        Canvas healthbar = Instantiate(canvas);
        healthbar.transform.position = tank_tower.transform.position;
        canvas_bar = healthbar.GetComponent<Canvas_Bar>();
        canvas_bar.Health_Bar_Update(current_health, max_health);
    }

    
    void Update()
    {
        if (_public_Var.GetComponent<Public_Variables_Class>().game_over)
        {
            Destroy(gameObject);
        }
        else
        {
            canvas_bar.Health_Bar_Update(current_health, max_health);
            canvas_bar.transform.position = tank_tower.transform.position;
            canvas_bar.transform.Translate(0, 2f, 0, Space.Self);
            canvas_bar.transform.rotation = tank_tower.transform.rotation;

            if (current_health <= 0)
            {
                Destroy(gameObject);
                _public_Var.GetComponent<Public_Variables_Class>().scores += 1;
                print("+1 очко");
            }

            Vector3 direction = player.position - transform.position;
            distance = direction.magnitude;

            if (tower_rotation)
                tank_tower.transform.rotation = Quaternion.RotateTowards(tank_tower.transform.rotation, Quaternion.LookRotation(direction), tower_rotationi_speed * Time.deltaTime);

            if (distance > 20)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), tank_rotation_speed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            Shot_Check();

            float tower_tank_angle = Vector3.Angle(tank_tower.transform.up, transform.forward);

            if (tower_tank_angle > 110 | tower_tank_angle < 70)
            {
                tower_rotation = false;
                print("Close");
            }
            else
                tower_rotation = true;

        }
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
        GameObject shell = Instantiate(shell_abstract);
        shell.GetComponent<Shell>().force = shot_power * 25;
        shell.GetComponent<Shell>().damage = shot_damage;
        shell.transform.position = tank_gun.transform.position;
        shell.transform.rotation = tank_gun.transform.rotation;
        shell.transform.Translate(0, 0, 1.5f, Space.Self);
        shell.GetComponent<Rigidbody>().AddRelativeForce(shell.transform.forward * shot_power, ForceMode.VelocityChange);
        Play_Audio(shot_clip);
        

    }

    private void Play_Audio(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

}
