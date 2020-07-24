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

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
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
