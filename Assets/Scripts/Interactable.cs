using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Interactable : MonoBehaviour
{
    private Outline outline;
    

   
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Enemy")
        {
            outline = GetComponent<Outline>();
            outline.OutlineWidth = 0;
            outline.OutlineColor = Color.red;
        }
    }

    public void OnHoverEnter()
    {
        if (gameObject.tag == "Enemy")
            outline.OutlineWidth = 6;
        
    }

    public void OnHoverExit()
    {
        if (gameObject.tag == "Enemy")
            outline.OutlineWidth = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
