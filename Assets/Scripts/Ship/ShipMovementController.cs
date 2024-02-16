using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementController : MonoBehaviour
{
    [Header("References")]
    [NonSerialized] public Rigidbody rb;
    [SerializeField] private ShipCameraController shipCamera;
    [SerializeField] private GameObject particles;

    [Header("Ship Movement")]
    [SerializeField] private float maxVelocity;
    [SerializeField] private float currentForwardVelocity;
    [SerializeField] private float kmPerHour;
    [Space]
    [SerializeField] private float maxPropulsion;
    [SerializeField] private float currentPropulsion;
    [SerializeField] private float propulsionDecay;
    [Space]
    [SerializeField] private float acceleration;
    [SerializeField] private float retardation;
    [SerializeField] private float movementInputWaitTime;
    private float forwardTimer;
    private bool canInput;
    [Space]
    [SerializeField] private float handbrakeSpeed;

    [Header("Ship Steering")]
    [SerializeField] private float shipRotLerpSpeed;
    private float xRot, yRot;
    private bool isFreeCam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canInput = true;
    }

    private void Update()
    {
        ShipInput();
        Movement();
        SpeedControl();
        Steering();

        if(kmPerHour > 2)
        {
            particles.SetActive(true);
        }
        else
        {
            particles.SetActive(false);
        }
    }

    private void ShipInput()
    {
        if(canInput == false) return;

        // Forward and Backwards 
        if(forwardTimer >= movementInputWaitTime)
        {
            if(Input.GetKey(KeyCode.W))
            {
                currentPropulsion += acceleration;

                if(currentPropulsion >= maxPropulsion)
                {
                    currentPropulsion = maxPropulsion;
                }

                forwardTimer = 0;
            }

            if(Input.GetKey(KeyCode.S))
            {
                currentPropulsion -= retardation;

                if(-currentPropulsion >= -maxPropulsion)
                {
                    currentPropulsion = -maxPropulsion;
                }

                forwardTimer = 0;    
            }
        }

        forwardTimer += Time.deltaTime;

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            currentPropulsion = 0;
        }

        // Handbrake
        if(Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, handbrakeSpeed * Time.deltaTime);
        }

        // Free Camera
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            xRot = shipCamera.XRotation;
            yRot = shipCamera.YRotation;

            isFreeCam = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            shipCamera.SetRotation(xRot, yRot);

            isFreeCam = false;
        }
    }

    private void Movement()
    {
        rb.AddForce(transform.forward * currentPropulsion, ForceMode.Acceleration);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);

        if(flatVel.magnitude > maxVelocity)
        {
            Vector3 limitedVel = flatVel.normalized * maxVelocity;
            rb.velocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
        }

        currentForwardVelocity = transform.InverseTransformDirection(rb.velocity).z;
        kmPerHour = currentForwardVelocity * 3.6f;
    }

    private void Steering()
    {
        if(isFreeCam == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - shipCamera.transform.position, shipCamera.transform.up), shipRotLerpSpeed * Time.deltaTime);
        }
    }

    public void Hit(Vector3 hitDir, float pushForce)
    {
        StopAllCoroutines();
        StartCoroutine(ShipHit(hitDir, pushForce));
    }

    private IEnumerator ShipHit(Vector3 hitDir, float pushForce)
    {
        canInput = false;

        rb.velocity = Vector3.zero;
        hitDir.Normalize();

        float timer = 0; 
        bool run = true;

        while(run)
        {
            rb.AddForce(hitDir * pushForce, ForceMode.Acceleration);

            timer += Time.deltaTime;
        
            if(1 >= timer)
            {
                run = false;
                canInput = true;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}