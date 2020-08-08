using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource rocketAudio;
    
    [SerializeField]float rcsThrust = 100f;
    [SerializeField]float mainThrust = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rocketAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("OK"); //todo remove
                break;
            case "Fuel":
                print("Fuel");
                break;
            default:
                print("Dead");
                break;
        }
    }

    void Thrust()
    {
        float rotationThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * rotationThisFrame);

            if (!rocketAudio.isPlaying)
                rocketAudio.Play();
        }
        else
        {
            if (rocketAudio.isPlaying)
                rocketAudio.Stop();
        }
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        
        //Rotate
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        
        rigidbody.freezeRotation = false;
    }
}
