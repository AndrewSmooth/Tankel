using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private float time_count = 0;
    public GameObject explosion_abstract;
    public GameObject yellow_explosion_abstract;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time_count += Time.deltaTime;
        if (time_count >= 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject explosion = Instantiate(explosion_abstract) as GameObject;
        explosion.transform.position = transform.position;
        GameObject y_explosion = Instantiate(yellow_explosion_abstract) as GameObject;
        y_explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}