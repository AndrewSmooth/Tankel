using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Mouse_Position : MonoBehaviour
{
    public Public_Variables_Class public_Variables_Class_Link;
    private Vector3 LastMousePosition;
    private int window_width;
    private int window_height;
    private float width_1_degree;
    private float height_1_degree;
    private float width_middle;
    private float height_horizon;
    private float right_left_angle;
    private float up_down_angle;

    void Start()
    {
        window_width = Screen.width;
        window_height = Screen.height;
        height_1_degree = (float)window_height / 45;
        width_1_degree = (float)window_width / 170; //85 в каждую сторону
        width_middle = (float)window_width / 2;
        height_horizon = 10 * height_1_degree;

    }

    void Update()
    {
        Mouse_Coors_Calc();
    }

    private void Mouse_Coors_Calc()
    {
        float x = Input.mousePosition.x + 0.1f; //Чтобы не стоял ровно посередине (Проблема с 360 градусов)
        float y = Input.mousePosition.y + 0.1f;
        if (x > Screen.width | y > Screen.height | x < 0 | y < 0)
        {
            return;
        }
        if (x <= width_middle)
        {
            right_left_angle = 360 - ((width_middle - x) / width_1_degree);
        }
        else
        {
            right_left_angle = (x - width_middle) / width_1_degree;
        }
        if (y <= height_horizon)
        {
            up_down_angle = (height_horizon - y) / height_horizon;
        }
        else
        {
            up_down_angle = 360 - (y - height_horizon) / height_1_degree;
        }
        public_Variables_Class_Link.tower_angle_vertical = up_down_angle;
        public_Variables_Class_Link.tower_angle_horizontal = right_left_angle;
    }
}