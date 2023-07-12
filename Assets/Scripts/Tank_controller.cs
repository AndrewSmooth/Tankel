using UnityEngine;

public class Tank_controller : MonoBehaviour
{
    public Public_Variables_Class public_Variables_Class_Link;
    public GameObject tank_abstract;
    private GameObject tank;
    private Vector3 start_tank_position = new Vector3(0f, 1f, 0f);

    private float tank_Move_Direction;
    private float tank_Rotation_Direction;
    private float tank_Move_Speed = 8f;
    private float tank_Rotation_Speed = 50f;


   

    private void Start()
    {
        tank = Instantiate(tank_abstract) as GameObject;
        tank.transform.position = start_tank_position;
    }

    void Update()
    {
        Tank_Movement();
    }

    private void Tank_Movement()
    {
        tank_Move_Direction = 0;
        tank_Rotation_Direction = 0;
        if (public_Variables_Class_Link.TurnUp)
        {
            tank_Move_Direction = 1;
        }
        if (public_Variables_Class_Link.TurnDown)
        {
            tank_Move_Direction = -1;
        }
        if (public_Variables_Class_Link.TurnRight)
        {
            if (tank_Move_Direction == -1)
            {
                tank_Rotation_Direction = -1;
            }
            else
            {
                tank_Rotation_Direction = 1;
            }
        }
        if (public_Variables_Class_Link.TurnLeft)
        {
            if (tank_Move_Direction == -1)
            {
                tank_Rotation_Direction = 1;
            }
            else
            {
                tank_Rotation_Direction = -1;
            }
        }

      
        tank.transform.Translate(0, 0, tank_Move_Direction * tank_Move_Speed * Time.deltaTime, Space.Self);      
        tank.transform.Rotate(0, tank_Rotation_Direction * tank_Rotation_Speed * Time.deltaTime, 0, Space.Self);
       
    
    }
    }