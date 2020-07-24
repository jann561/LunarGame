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

        if (Input.GetButton("Jump"))
        {
            RocketForce = RocketForce + Time.deltaTime * 6;
            if (RocketForce < 9.81f)
            {
                RocketForce = RocketForce + Time.deltaTime * 50;
            }
        }

        //Auto reduce RocketForce variable and limit to >0
        //Apply Gravity force and Rocket force
        RocketForce = RocketForce - Time.deltaTime * 4;
        if (RocketForce < 0)
        {
            RocketForce = 0;
        }

        r.AddForce(Physics.gravity/4);
        r.AddForce(0, (RocketForce/4), 0);
    }
}
