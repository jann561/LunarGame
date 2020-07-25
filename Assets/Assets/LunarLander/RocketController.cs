using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public float Speed;
    public float AngularSpeed;
    public float RocketForce;
    public float RotationSpeed;
    protected Rigidbody r;
    public ParticleSystem thruster1;
    private ParticleSystem.EmissionModule thruster1_emission;
    public ParticleSystem thruster2;
    private ParticleSystem.EmissionModule thruster2_emission;
    public ParticleSystem thruster3;
    private ParticleSystem.EmissionModule thruster3_emission;
    public ParticleSystem thruster4;
    private ParticleSystem.EmissionModule thruster4_emission;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        thruster1 = this.transform.Find("Thruster1").GetComponent<ParticleSystem>();
        thruster2 = this.transform.Find("Thruster2").GetComponent<ParticleSystem>();
        thruster3 = this.transform.Find("Thruster3").GetComponent<ParticleSystem>();
        thruster4 = this.transform.Find("Thruster4").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Speed = r.velocity.magnitude;
        AngularSpeed = r.angularVelocity.magnitude;

        //If boots is pressed add force
        //Else remove force
        if (Input.GetButton("Jump"))
        {
            RocketForce = RocketForce + Time.deltaTime * 6;
            if (RocketForce < 9.81f)
            {
                RocketForce = RocketForce + Time.deltaTime * 50;
            }
        }
        else
        {
            RocketForce = RocketForce - Time.deltaTime * 20;
        }
        thruster1_emission = thruster1.emission;
        thruster1_emission.rateOverTime = RocketForce;
        thruster2_emission = thruster2.emission;
        thruster2_emission.rateOverTime = RocketForce;
        thruster3_emission = thruster3.emission;
        thruster3_emission.rateOverTime = RocketForce;
        thruster4_emission = thruster4.emission;
        thruster4_emission.rateOverTime = RocketForce;

        //Rotate ship
        if (Input.GetAxis("Horizontal") != 0)
        {
            r.AddRelativeTorque(0, 0, -1 * (Input.GetAxis("Horizontal") * RotationSpeed), mode:ForceMode.Acceleration);
        }

        //Limit RocketForce to >0
        if (RocketForce < 0)
        {
            RocketForce = 0;
        }

        //Apply Gravity force and Rocket force
        r.AddForce(Physics.gravity/4);
        r.AddForce(0, (RocketForce/4), 0);
    }
}
