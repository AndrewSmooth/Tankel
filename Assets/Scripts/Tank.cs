
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tank : MonoBehaviour
{
    private GameObject _public_Var;
    private GameObject tank_tower;
    private GameObject tank_fictive_gun;
    private GameObject tank_gun;
    private GameObject tank_gun_camera;
    private GameObject tank_back_view_camera;

    public GameObject shell_abstract;
    private float shot_power = 40f;
    private float shot_damage = 25f;
    private float max_health = 1000f;
    public float current_health;
    public AudioClip shot_clip;
    public bool shot_button_clicked = false;
    public bool shot_allowed = true;
    private float shot_cooldown_count = 0f;
    

    public Canvas canvas;
    public Canvas game_over_window_abstract;
    private Canvas_Bar canvas_bar;


    void Start()
    {
        current_health = max_health;
        tank_tower = transform.GetChild(0).gameObject;
        _public_Var = GameObject.Find("___public_variables_class");
        tank_fictive_gun = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        tank_gun = tank_fictive_gun.transform.GetChild(0).gameObject;
        tank_back_view_camera = transform.GetChild(1).gameObject;
        tank_gun_camera = tank_gun.transform.GetChild(0).gameObject;
        tank_fictive_gun.transform.Rotate(-10f, 0, 0, Space.Self);

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
            
            Camera_Switch();
            Tower_and_Gun_Rotation();
            Shot_Check();
            canvas_bar.Health_Bar_Update(current_health, max_health);
            canvas_bar.transform.position = tank_tower.transform.position;
            canvas_bar.transform.Translate(0f, -0.6f, 2f, Space.Self);
            Vector3 direction = tank_back_view_camera.transform.position - canvas_bar.transform.position;
            canvas_bar.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
        
    private void Tower_and_Gun_Rotation()
    {
        tank_tower.transform.localEulerAngles = new Vector3(0, _public_Var.GetComponent<Public_Variables_Class>().tower_angle_horizontal);
        tank_fictive_gun.transform.localEulerAngles = new Vector3(_public_Var.GetComponent<Public_Variables_Class>().tower_angle_vertical, 0);    
    }

    private void Camera_Switch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        { 
        if (tank_gun_camera.GetComponent<Camera>().depth == 0)
        {
            tank_gun_camera.GetComponent<Camera>().depth = 1;
            tank_back_view_camera.GetComponent<Camera>().depth = 0;
        }
        else
        {
            tank_gun_camera.GetComponent<Camera>().depth = 0;
            tank_back_view_camera.GetComponent<Camera>().depth = 1;
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
