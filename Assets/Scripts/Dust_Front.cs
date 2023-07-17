using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust_Front : MonoBehaviour
{
    private ParticleSystem ps;
    public float rateOverDistanceValue;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var emission = ps.emission;
        emission.rateOverDistanceMultiplier = rateOverDistanceValue;
    }
}
