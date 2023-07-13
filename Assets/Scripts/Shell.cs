using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public Public_Variables_Class public_Variables_Class_Link;
    private float time_count = 0;
    public GameObject explosion_abstract;
    public GameObject yellow_explosion_abstract;

    public float force;
    public float damage;
    public Rigidbody rb;

    void Start()
    {
        
    }

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
        if (other.transform.tag == "Enemy")
        {
            other.GetComponent<Enemy>().current_health -= damage;
            rb = other.GetComponent<Rigidbody>();
            rb.AddForceAtPosition(transform.forward * force, transform.position);
        }
        if (other.transform.tag == "Player")
        {
            other.GetComponent<Tank>().current_health -= damage;
            rb = other.GetComponent<Rigidbody>();
            rb.AddForceAtPosition(transform.forward * force, transform.position);
        }
        GameObject explosion = Instantiate(explosion_abstract) as GameObject;
        explosion.transform.position = transform.position;
        GameObject y_explosion = Instantiate(yellow_explosion_abstract) as GameObject;
        y_explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}