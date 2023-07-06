using UnityEngine;

public class Tank_controller_panel : MonoBehaviour
{
    public Public_Variables_Class public_Variables_Class;


    void Update()
    {
        Keys_Listener();
    }

    private void Keys_Listener()
    {
        if (Input.GetKey(KeyCode.W) & !Input.GetKey(KeyCode.S))
        {
            public_Variables_Class.TurnUp = true;
        }
        else
        {
            public_Variables_Class.TurnUp = false;
        }
        if (Input.GetKey(KeyCode.S) & !Input.GetKey(KeyCode.W))
        {
            public_Variables_Class.TurnDown = true;
        }
        else
        {
            public_Variables_Class.TurnDown = false;
        }
        if (Input.GetKey(KeyCode.A) & !Input.GetKey(KeyCode.D))
        {
            public_Variables_Class.TurnLeft = true;
        }
        else
        {
            public_Variables_Class.TurnLeft = false;
        }
        if (Input.GetKey(KeyCode.D) & !Input.GetKey(KeyCode.A))
        {
            public_Variables_Class.TurnRight = true;
        }
        else
        {
            public_Variables_Class.TurnRight = false;
        }
        if (!Input.GetKey(KeyCode.D) & !Input.GetKey(KeyCode.A) & !Input.GetKey(KeyCode.W) & !Input.GetKey(KeyCode.S))
        {
            public_Variables_Class.TurnDown = false;
            public_Variables_Class.TurnUp = false;
            public_Variables_Class.TurnLeft = false;
            public_Variables_Class.TurnRight = false;
        }
    }
}
