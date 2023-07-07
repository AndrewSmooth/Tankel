using UnityEngine;

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
    public AudioClip shot_clip;
    public bool shot_button_clicked = false;
    public bool shot_allowed = true;
    private float shot_cooldown_count = 0f;

    
   
    void Start()
    {
        _public_Var = GameObject.Find("___public_variables_class");

        tank_tower = transform.GetChild(0).gameObject;

        tank_fictive_gun = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        tank_gun = tank_fictive_gun.transform.GetChild(0).gameObject;
        tank_back_view_camera = transform.GetChild(1).gameObject;
        tank_gun_camera = tank_gun.transform.GetChild(0).gameObject;
        tank_fictive_gun.transform.Rotate(-10f, 0, 0, Space.Self);
    }

   
    void Update()
    {
        Camera_Switch();
        Tower_and_Gun_Rotation();
        Shot_Check();
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
        GameObject shell = Instantiate(shell_abstract) as GameObject;
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
