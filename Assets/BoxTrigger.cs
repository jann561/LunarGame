using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Event " + other.name);
    }

    private void onCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
    }
}
