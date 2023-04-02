using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TireSmoke : MonoBehaviour
{
    public ParticleSystem[] driftSmoke;
    public ParticleSystem[] driveSmoke;
    public ParticleSystem[] boostFlame;

    public CarController controller;
    public float smokeRate;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CarController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        playSmoke(controller.carSpeed());
    }

    public void playSmoke(float carSpeed) {

        for (int i=0; i<driftSmoke.Length; i++) {
            var emission = driftSmoke[i].emission;
            emission.enabled = true;
            emission.rateOverTime = smokeRate * (controller.carSpeed() - 40) * Math.Abs(controller.getTurnInput());
        }

        for (int i=0; i<driveSmoke.Length; i++) {
            var emission = driveSmoke[i].emission;
            emission.enabled = true;
            emission.rateOverTime = smokeRate/2 * (controller.carSpeed() - 20) * (1 - Math.Abs(controller.getTurnInput()));
        }

        for (int i=0; i<boostFlame.Length; i++) {
            var emission = boostFlame[i].emission;
            emission.enabled = true;
            emission.rateOverTime = smokeRate * 5 * ((controller.getIsBoost()) ? 1 : 0);
        }
        
        
        
    }

}