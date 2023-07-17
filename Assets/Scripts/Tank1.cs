
using Palmmedia.ReportGenerator.Core;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tank1 : MonoBehaviour
{
    private GameObject _public_Var;
    private GameObject tank_tower;
    private GameObject tank_back_view_camera;
    private GameObject tank_left_caterpillar;
    private GameObject tank_right_caterpillar;
    private GameObject auto_aim;
    public GameObject shell_abstract;
    private Vector3 target;

    public float shot_power = 40f;
    private float shot_damage = 25f;
    private float max_health = 1000f;
    private float shot_cooldown_count = 0f;
    private float cam_xrot_offset = 0.1f;
    private float cam_ypos_offset = 0.1f;
    private float cam_zpos_offset = 0.2f;
    public float current_health;
    private float dust_straight_value = 50f; 
    private float dust_rotation_value = 10f;
    

    public bool shot_button_clicked = false;
    public bool shot_allowed = true;
    private bool enemy_detected = false;

    private Interactable previousInteractable;
    private Interactable interactable;


    public AudioClip shot_clip;

    private Canvas_Bar canvas_bar;

    public Canvas canvas;


    void Start()
    {
        

        current_health = max_health;
        tank_tower = transform.GetChild(0).gameObject;
        _public_Var = GameObject.Find("___public_variables_class");
        tank_back_view_camera = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        tank_left_caterpillar = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        tank_right_caterpillar = transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        auto_aim = tank_tower.transform.GetChild(1).gameObject;
        tank_back_view_camera.transform.Rotate(20*cam_xrot_offset, 0f, 0f);
        tank_back_view_camera.transform.Translate(0f, 20*cam_ypos_offset, -20*cam_zpos_offset);
        

        tank_left_caterpillar.GetComponent<Animator>().speed = 5;
        tank_right_caterpillar.GetComponent<Animator>().speed = 5;




        Vector3 direction = tank_tower.transform.position - tank_back_view_camera.transform.position;
        tank_back_view_camera.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);


        Canvas healthbar = Instantiate(canvas);
        healthbar.transform.position = tank_back_view_camera.transform.position;
        canvas_bar = healthbar.GetComponent<Canvas_Bar>();
        canvas_bar.Health_Bar_Update(current_health, max_health);
    }


    void Update()
    {
        

        if (_public_Var.GetComponent<Public_Variables_Class>().game_over | current_health <= 0)
        {
            if (current_health <= 0)
                SceneManager.LoadScene("DeathScene");
            else
                SceneManager.LoadScene("GameOverScene");

        }
        else
        {
            Raycast();
            Tower_Rotation();
            Shot_Check();
            Tank_Animation();
            Camera_Switch();
            //+ _public_Var.GetComponent<Public_Variables_Class>().scores
            //scores_canvas.GetComponent<TextMeshPro>().text;
            canvas_bar.Health_Bar_Update(current_health, max_health);
            canvas_bar.transform.position = tank_tower.transform.position;
            canvas_bar.transform.Translate(0f, 0.1f, 1f, Space.Self);
            Vector3 direction = tank_back_view_camera.transform.position - canvas_bar.transform.position;
            canvas_bar.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void Tower_Rotation()
    {
        tank_tower.transform.localEulerAngles = new Vector3(0, _public_Var.GetComponent<Public_Variables_Class>().tower_angle_horizontal);
    }


    private void Camera_Switch()
    {
        
        if (Input.GetKey(KeyCode.F))
        {
            if (tank_back_view_camera.transform.position.y < 11f)
            {
                tank_back_view_camera.transform.Rotate(cam_xrot_offset, 0f, 0f);
                tank_back_view_camera.transform.Translate(0f, cam_ypos_offset, -cam_zpos_offset);
                Vector3 direction = tank_tower.transform.position - tank_back_view_camera.transform.position;
                tank_back_view_camera.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);  
            }
        }

        if (Input.GetKey(KeyCode.G))
        {
            if (tank_back_view_camera.transform.position.y > 1.1f)
            {
                tank_back_view_camera.transform.Rotate(-cam_xrot_offset, 0f, 0f);
                tank_back_view_camera.transform.Translate(0f, -cam_ypos_offset, cam_zpos_offset);
                Vector3 direction = tank_tower.transform.position - tank_back_view_camera.transform.position;
                tank_back_view_camera.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

    }

    private void Shot_Check()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shot_button_clicked = true;
        }
        else
        {
            shot_button_clicked = false;
        }
        shot_cooldown_count += Time.deltaTime;
        if (shot_cooldown_count >= _public_Var.GetComponent<Public_Variables_Class>().shot_cooldown)
        {
            shot_allowed = true;
        }
        if (shot_button_clicked & shot_allowed)
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


    private void Tank_Animation()
    {
        Animator l_cat_anim = tank_left_caterpillar.GetComponent<Animator>();
        Animator r_cat_anim = tank_right_caterpillar.GetComponent<Animator>();
        Dust_Front l_dust_front = tank_left_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Dust_Front>();
        Dust_Front r_dust_front = tank_right_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Dust_Front>();
        Dust_Front l_dust_behind = tank_left_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Dust_Front>();
        Dust_Front r_dust_behind = tank_right_caterpillar.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Dust_Front>();

        if (_public_Var.GetComponent<Public_Variables_Class>().TurnUp | _public_Var.GetComponent<Public_Variables_Class>().TurnDown)
        {
            if (_public_Var.GetComponent<Public_Variables_Class>().TurnUp)
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
                l_cat_anim.SetBool("isBack", true);
                r_cat_anim.SetBool("isForward", false);
                r_cat_anim.SetBool("isBack", true);
                l_dust_front.rateOverDistanceValue = dust_straight_value;
                l_dust_behind.rateOverDistanceValue = 0f;
                r_dust_front.rateOverDistanceValue = dust_straight_value;
                r_dust_behind.rateOverDistanceValue = 0f;
            }
        }

        else
        {
            if (_public_Var.GetComponent<Public_Variables_Class>().TurnLeft)
            {
                l_cat_anim.SetBool("isForward", false);
                l_cat_anim.SetBool("isBack", true);
                r_cat_anim.SetBool("isForward", true);
                r_cat_anim.SetBool("isBack", false);
                l_dust_front.rateOverDistanceValue = dust_rotation_value;
                l_dust_behind.rateOverDistanceValue = 0f;
                r_dust_front.rateOverDistanceValue = 0f;
                r_dust_behind.rateOverDistanceValue = dust_rotation_value;          
            }
            else if (_public_Var.GetComponent<Public_Variables_Class>().TurnRight)
            {
                l_cat_anim.SetBool("isForward", true);
                l_cat_anim.SetBool("isBack", false);
                r_cat_anim.SetBool("isForward", false);
                r_cat_anim.SetBool("isBack", true);
                l_dust_front.rateOverDistanceValue = 0f;
                l_dust_behind.rateOverDistanceValue = dust_rotation_value;
                r_dust_front.rateOverDistanceValue = dust_rotation_value;
                r_dust_behind.rateOverDistanceValue = 0f;              
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


    private void Raycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(auto_aim.transform.position, auto_aim.transform.forward * 120f);
        print("Первый луч выпущен");
        Debug.DrawRay(auto_aim.transform.position, auto_aim.transform.forward * 120f, Color.blue, 5);
        if (Physics.Raycast(ray, out hit) & hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                enemy_detected = true;
                target = hit.point;
                Light_Enemy(hit.collider.gameObject);
            }
            else
            {
                print("Это не enemy");
                enemy_detected = false;
                if (previousInteractable != null)
                {
                    previousInteractable.OnHoverExit();
                }

                for (float i = 1f; i < 12f; i += 1f)
                {
                    if (i < 9f & enemy_detected != true)
                    {
                        auto_aim.transform.Rotate(-i, 0f, 0f);
                        Ray ray1 = new Ray(auto_aim.transform.position, auto_aim.transform.forward * 120f);
                        print("Выпущен новый луч с i" + i);
                        Debug.DrawRay(auto_aim.transform.position, auto_aim.transform.forward * 120f, Color.blue, 5);
                        if (Physics.Raycast(ray1, out hit) & hit.collider != null)
                        {
                            if (hit.collider.gameObject.tag == "Enemy")
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
                    

                    if (enemy_detected != true & i < 12f)
                    {
                        auto_aim.transform.Rotate(i, 0f, 0f);
                        Ray ray2 = new Ray(auto_aim.transform.position, auto_aim.transform.forward * 120f);
                        Debug.DrawRay(auto_aim.transform.position, auto_aim.transform.forward * 120f, Color.blue, 5);
                        if (Physics.Raycast(ray2, out hit) & hit.collider != null)
                        {
                            if (hit.collider.gameObject.tag == "Enemy")
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
            print("Это не колайдер");
            enemy_detected = false;
            if (previousInteractable != null)
            {
                previousInteractable.OnHoverExit();
            }
        }
        
    }

    public void Light_Enemy(GameObject enemy)
    {
        print("hitted");
        if (previousInteractable != null)
        {
            previousInteractable.OnHoverExit();
        }
        interactable = enemy.GetComponent<Interactable>();
        interactable.OnHoverEnter();
        previousInteractable = interactable;
    }
        
    }

