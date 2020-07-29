using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    private const float SafeSpeedThreshold = 15f;
    public float Speed;
    public float AngularSpeed;
    public float RocketForce;
    public float RotationSpeed;
    private float Fuel;
    public Slider slider;
    private float relative_direction;
    protected Rigidbody rigid_body;
    private float emission_rate;
    private bool game_over;
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

    private CanvasGroup crash_overlay;
    private CanvasGroup win_overlay;
    private CanvasGroup nofuel_overlay;

    // Start is called before the first frame update
    void Start()
    {
        rigid_body = GetComponent<Rigidbody>();
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

        crash_overlay = self_transform.Find("Canvas").Find("CrashDisplay").GetComponent<CanvasGroup>();
        crash_overlay.alpha = 0;
        crash_overlay.interactable = false;

        win_overlay = self_transform.Find("Canvas").Find("WinDisplay").GetComponent<CanvasGroup>();
        win_overlay.alpha = 0;
        win_overlay.interactable = false;

        nofuel_overlay = self_transform.Find("Canvas").Find("NoFuelDisplay").GetComponent<CanvasGroup>();
        nofuel_overlay.alpha = 0;
        nofuel_overlay.interactable = false;

        Fuel = 40;
        game_over = false;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = rigid_body.velocity.magnitude;
        AngularSpeed = rigid_body.angularVelocity.magnitude;

        //If boots is pressed add force
        //Else remove force
        if (Input.GetButton("Jump") && (Fuel > 0))
        {
            RocketForce = Math.Min((RocketForce + Time.deltaTime * 15), 30);
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
        slider.value = (Fuel / 40);


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
        rigid_body.AddForce(Physics.gravity/4);
        rigid_body.AddForce((RocketForce / 4 * (1 * (relative_direction / 45))), (RocketForce / 4 * (1 - (Mathf.Abs(relative_direction)/45))), 0);

        if (Fuel == 0)
        {
            if (game_over == false)
            {
                nofuel_overlay.alpha = 1;
                nofuel_overlay.interactable = true;
                game_over = true;
                nofuel_overlay.enabled = true;
                win_overlay.enabled = true;
                crash_overlay.enabled = true;

                nofuel_overlay.blocksRaycasts = true;
                win_overlay.blocksRaycasts = false;
                crash_overlay.blocksRaycasts = false;
            }
        }
    }

    //Check for at landing position
    private void OnTriggerEnter(Collider other)
    {
        if (Speed < SafeSpeedThreshold)
        {
            if (game_over == false)
            {
                win_overlay.alpha = 1;
                win_overlay.interactable = true;
                game_over = true;
                Fuel = 0;
                nofuel_overlay.enabled = true;
                win_overlay.enabled = true;
                crash_overlay.enabled = true;

                nofuel_overlay.blocksRaycasts = false;
                win_overlay.blocksRaycasts = true;
                crash_overlay.blocksRaycasts = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Speed > SafeSpeedThreshold)
        {
            if (game_over == false)
            {
                crash_overlay.alpha = 1;
                crash_overlay.interactable = true;
                game_over = true;
                Fuel = 0;
                nofuel_overlay.enabled = true;
                win_overlay.enabled = true;
                crash_overlay.enabled = true;
                nofuel_overlay.blocksRaycasts = false;
                win_overlay.blocksRaycasts = false;
                crash_overlay.blocksRaycasts = true;
            }
        }
    }
}
