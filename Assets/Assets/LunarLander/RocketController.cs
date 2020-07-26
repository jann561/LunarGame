using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    public float Speed;
    public float AngularSpeed;
    public float RocketForce;
    public float RotationSpeed;
    private float Fuel;
    public Slider slider;
    private float relative_direction;
    protected Rigidbody r;
    private float emission_rate;
    private Transform self_transform;
    private Transform thruster1;
    private ParticleSystem thruster1p;
    private ParticleSystem.EmissionModule thruster1_emission;
    private Transform thruster2;
    private ParticleSystem thruster2p;
    private ParticleSystem.EmissionModule thruster2_emission;
    private Transform thruster3;
    private ParticleSystem thruster3p;
    private ParticleSystem.EmissionModule thruster3_emission;
    private Transform thruster4;
    private ParticleSystem thruster4p;
    private ParticleSystem.EmissionModule thruster4_emission;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        self_transform = this.transform;
        slider = self_transform.Find("Canvas").Find("Slider").GetComponent<Slider>();
        thruster1 = self_transform.Find("Thruster1");
        thruster1p = thruster1.GetComponent<ParticleSystem>();
        thruster2 = self_transform.Find("Thruster2");
        thruster2p = thruster2.GetComponent<ParticleSystem>();
        thruster3 = self_transform.Find("Thruster3");
        thruster3p = thruster3.GetComponent<ParticleSystem>();
        thruster4 = self_transform.Find("Thruster4");
        thruster4p = thruster4.GetComponent<ParticleSystem>();

        Fuel = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = r.velocity.magnitude;
        AngularSpeed = r.angularVelocity.magnitude;

        //If boots is pressed add force
        //Else remove force
        if (Input.GetButton("Jump") && (Fuel > 0))
        {
            RocketForce = RocketForce + Time.deltaTime * 10;
            if (RocketForce < 9.81f)
            {
                RocketForce = RocketForce + Time.deltaTime * 50;
            }
        }
        else
        {
            RocketForce = Math.Max((RocketForce - Time.deltaTime * 50),0);
        }
        emission_rate = 10f;
        thruster1_emission = thruster1p.emission;
        thruster1_emission.rateOverTime = RocketForce * emission_rate;
        thruster2_emission = thruster2p.emission;
        thruster2_emission.rateOverTime = RocketForce * emission_rate;
        thruster3_emission = thruster3p.emission;
        thruster3_emission.rateOverTime = RocketForce * emission_rate;
        thruster4_emission = thruster4p.emission;
        thruster4_emission.rateOverTime = RocketForce * emission_rate;

        //Reduce fuel when boosting
        Fuel = Math.Max((Fuel - Time.deltaTime * RocketForce / 4),0);
        slider.value = (Fuel / 100);


        //Rotate ship
        if (Input.GetAxis("Horizontal") != 0)
        {
            relative_direction = (relative_direction + Input.GetAxis("Horizontal") / 4);
            relative_direction = Math.Min(relative_direction, 45f);
            relative_direction = Math.Max(relative_direction, -45f);

            thruster1.eulerAngles = new Vector3(0f, 0f, -1 * relative_direction);
            thruster2.eulerAngles = new Vector3(0f, 0f, -1 * relative_direction);
            thruster3.eulerAngles = new Vector3(0f, 0f, -1 * relative_direction);
            thruster4.eulerAngles = new Vector3(0f, 0f, -1 * relative_direction);
        }

        //Limit RocketForce to >0
        if (RocketForce < 0)
        {
            RocketForce = 0;
        }

        //Apply Gravity force and Rocket force
        r.AddForce(Physics.gravity/4);
        r.AddForce((RocketForce / 4 * (1 * (relative_direction / 45))), (RocketForce / 4 * (1 - (Mathf.Abs(relative_direction)/45))), 0);
    }
}
