using UnityEngine;

public class Tank : MonoBehaviour
{
    private GameObject _public_Var;
    private GameObject tank_tower;
    private GameObject tank_fictive_gun;
    private GameObject tank_gun;
    private GameObject tank_gun_camera;
    private GameObject tank_back_view_camera;
    
   
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
}
