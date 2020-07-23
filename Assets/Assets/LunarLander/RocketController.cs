﻿using System.Collections;
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
        }
        RocketForce = RocketForce - Time.deltaTime * 4;

        r.AddForce(Physics.gravity/4);
        r.AddForce(0, (RocketForce/4), 0);
    }
}
