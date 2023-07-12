using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Bar : MonoBehaviour
{
    public Image bar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Health_Bar_Update(float current_health, float max_health)
    {
        if (current_health <= 0)
        {
            Destroy(gameObject); return;
        }
        bar = transform.GetChild(0).gameObject.GetComponent<Image>();
        bar.fillAmount = current_health / max_health;
    }

}
