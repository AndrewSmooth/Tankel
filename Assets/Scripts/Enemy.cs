using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private GameObject _public_Var;
    private GameObject tank_tower;
    private GameObject tank_left_caterpillar;
    private GameObject tank_right_caterpillar;
    private GameObject auto_aim;
    public GameObject shell_abstract;
    private Vector3 target;

    private float shot_power = 40f;
    private float shot_damage = 25f;
    private float max_health = 100f;
    private float shot_cooldown_count = 0f;
    private float speed = 5f;
    public float current_health;
    private float dust_straight_value = 50f;

    public bool shot_allowed = true;
    private bool enemy_detected = false;

    private Interactable previousInteractable;
    private Interactable interactable;

    public AudioClip shot_clip;

    private Canvas_Bar canvas_bar;

    public Canvas canvas;

    
    public GameObject player;

    private float distance;
    public bool tower_rotation = true;
    public float tower_rotation_speed = 50;
    public float tank_rotation_speed = 100;

    public float animation_time = 0;



    void Start()
    {
        current_health = max_health;
        tank_tower = transform.GetChild(0).gameObject;
        _public_Var = GameObject.Find("___public_variables_class");
        player = GameObject.FindGameObjectWithTag("Player");
        tank_left_caterpillar = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        tank_right_caterpillar = transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        auto_aim = tank_tower.transform.GetChild(1).gameObject;

        

        tank_left_caterpillar.GetComponent<Animator>().speed = 5;
        tank_right_caterpillar.GetComponent<Animator>().speed = 5;

        Canvas healthbar = Instantiate(canvas);
        healthbar.transform.position = tank_tower.transform.position;
        canvas_bar = healthbar.GetComponent<Canvas_Bar>();
        canvas_bar.Health_Bar_Update(current_health, max_health);
    }


    void Update()
    {
        Animation(false);

        player = GameObject.FindGameObjectWithTag("Player");

        if (_public_Var.GetComponent<Public_Variables_Class>().game_over | current_health <= 0)
        {
            Destroy(gameObject);
            if (current_health <= 0)
            {
                Update_Canvas();
                _public_Var.GetComponent<Public_Variables_Class>().scores += 1;
                print("+1 очко");
            }
        }
        else
        {
            Tower_Rotation();
            Movement();
            Raycast();
            Shot_Check();
            //Animation();
            Bug_Fixed(); //Баг положения башни при сильном отталкивании
            Update_Canvas();


        }
    }


    private void Raycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(auto_aim.transform.position, auto_aim.transform.forward * 120f);
        if (Physics.Raycast(ray, out hit) & hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (previousInteractable != null)
                {
                    previousInteractable.OnHoverExit();
                }

                for (float i = 1f; i < 12f; i += 1f)
                {
                    if (i < 9f)
                    {
                        auto_aim.transform.Rotate(-i, 0f, 0f);
                        Ray ray1 = new Ray(auto_aim.transform.position, auto_aim.transform.forward * 120f);
                        Debug.DrawRay(auto_aim.transform.position, auto_aim.transform.forward * 120f, Color.blue, 5);
                        if (Physics.Raycast(ray1, out hit) & hit.collider != null)
                        {
                            if (hit.collider.gameObject.tag == "Player")
                            {
                                enemy_detected = true;
                                target = hit.point;
                                Light_Enemy(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            enemy_detected = false;
                            if (previousInteractable != null)
                            {
                                previousInteractable.OnHoverExit();
                            }
                        }
                        auto_aim.transform.Rotate(i, 0f, 0f);
                    }

                    if (enemy_detected != true & i <= 11f)
                    {
                        auto_aim.transform.Rotate(i, 0f, 0f);
                        Ray ray1 = new Ray(auto_aim.transform.position, auto_aim.transform.forward * 120f);
                        Debug.DrawRay(auto_aim.transform.position, auto_aim.transform.forward * 120f, Color.blue, 5);
                        if (Physics.Raycast(ray1, out hit) & hit.collider != null)
                        {
                            if (hit.collider.gameObject.tag == "Player")
                            {
                                enemy_detected = true;
                                target = hit.point;
                                Light_Enemy(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            enemy_detected = false;
                            if (previousInteractable != null)
                            {
                                previousInteractable.OnHoverExit();
                            }
                        }
                        auto_aim.transform.Rotate(-i, 0f, 0f);
                    }

                }
            }
            
        }
        else
        {
            enemy_detected = true;
            target = hit.point;
            Light_Enemy(hit.collider.gameObject);

        }
    }


    public void Light_Enemy(GameObject enemy)
    {
        if (previousInteractable != null)
        {
            previousInteractable.OnHoverExit();
        }
        interactable = enemy.GetComponent<Interactable>();
        previousInteractable = interactable;
    }


    private void Shot_Check()
    {
        shot_cooldown_count += Time.deltaTime;
        if (shot_cooldown_count >= _public_Var.GetComponent<Public_Variables_Class>().shot_cooldown & enemy_detected)
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
        shell.GetComponent<Shell>().force = shot_power * 10;
        shell.GetComponent<Shell>().damage = shot_damage;
        shell.transform.position = tank_tower.transform.position;
        if (enemy_detected)
        {
            Vector3 direction = -(tank_tower.transform.position - target);
            shell.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            shell.transform.rotation = tank_tower.transform.rotation;
        }

        shell.transform.Translate(0, 0, 3f, Space.Self);
        shell.GetComponent<Rigidbody>().AddRelativeForce(shell.transform.forward * shot_power, ForceMode.VelocityChange);
        Play_Audio(shot_clip);
    }

    private void Play_Audio(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);

    }

    private void Tower_Rotation()
    {
        if (tower_rotation)
        {
            Vector3 direction = player.transform.position - transform.position;
            distance = direction.magnitude;
            tank_tower.transform.rotation = Quaternion.RotateTowards(tank_tower.transform.rotation, Quaternion.LookRotation(direction), tower_rotation_speed * Time.deltaTime);
        }
    }


    private void Update_Canvas()
    {

        canvas_bar.Health_Bar_Update(current_health, max_health);
        canvas_bar.transform.position = tank_tower.transform.position;
        canvas_bar.transform.Translate(0, 2f, 0, Space.Self);
        canvas_bar.transform.rotation = tank_tower.transform.rotation;
    }


    private void Movement()
    {
        if (distance > 20)
        {
            Vector3 direction = player.transform.position - transform.position;
            distance = direction.magnitude;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), tank_rotation_speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            Animation(true);
        }
    }


    private void Bug_Fixed()
    {
        float tower_tank_angle = Vector3.Angle(tank_tower.transform.up, transform.forward);

        if (tower_tank_angle > 110 | tower_tank_angle < 70)
        {
            tower_rotation = false;
        }
        else
            tower_rotation = true;
    }


    private void Animation(bool go)
    {

        Animator l_cat_anim = tank_left_caterpillar.GetComponent<Animator>();
        Animator r_cat_anim = tank_right_caterpillar.GetComponent<Animator>();
        Dust_Front l_dust_front = tank_left_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Dust_Front>();
        Dust_Front r_dust_front = tank_right_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Dust_Front>();
        Dust_Front l_dust_behind = tank_left_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Dust_Front>();
        Dust_Front r_dust_behind = tank_right_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Dust_Front>();

        animation_time += Time.deltaTime;

        if (go)
        {
            l_cat_anim.SetBool("isForward", true);
            l_cat_anim.SetBool("isBack", false);
            r_cat_anim.SetBool("isForward", true);
            r_cat_anim.SetBool("isBack", false);
            l_dust_front.rateOverDistanceValue = 0f;
            l_dust_behind.rateOverDistanceValue = dust_straight_value;
            r_dust_front.rateOverDistanceValue = 0f;
            r_dust_behind.rateOverDistanceValue = dust_straight_value;
        }
           
            else
            {
                l_cat_anim.SetBool("isForward", false);
                l_cat_anim.SetBool("isBack", false);
                r_cat_anim.SetBool("isForward", false);
                r_cat_anim.SetBool("isBack", false);
                l_dust_front.rateOverDistanceValue = 0f;
                l_dust_behind.rateOverDistanceValue = 0f;
                r_dust_front.rateOverDistanceValue = 0f;
                r_dust_behind.rateOverDistanceValue = 0f;
            }
        
    }
}
