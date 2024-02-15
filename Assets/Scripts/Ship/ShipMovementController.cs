using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    public Transform shipCamera;

    [Header("Ship Movement")]
    [SerializeField] private float maxVelocity;
    [SerializeField] private float currentForwardVelocity;
    [SerializeField] private float kmPerHour;
    [Space]
    [SerializeField] private float maxPropulsion;
    [SerializeField] private float currentPropulsion;
    [SerializeField] private float propulsionDecay;
    public bool decayActive;
    [Space]
    [SerializeField] private float acceleration;
    [SerializeField] private float retardation;
    [SerializeField] private float movementInputWaitTime;
    private float forwardTimer;

    [Header("Ship Steering")]
    [SerializeField] private float shipRotLerpSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ShipInput();
        Movement();
        SpeedControl();
        Steering();
    }

    private void ShipInput()
    {
        if(forwardTimer >= movementInputWaitTime)
        {
            if(Input.GetKey(KeyCode.W))
            {
                currentPropulsion += acceleration;

                if(currentPropulsion >= maxPropulsion)
                {
                    currentPropulsion = maxPropulsion;
                }

                decayActive = false;
                forwardTimer = 0;
            }

            if(Input.GetKey(KeyCode.S))
            {
                currentPropulsion -= retardation;

                if(-currentPropulsion >= -maxPropulsion)
                {
                    currentPropulsion = -maxPropulsion;
                }

                decayActive = false;
                forwardTimer = 0;    
            }
        }

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            decayActive = true;
        }

        forwardTimer += Time.deltaTime;
    }

    private void Movement()
    {
        rb.AddForce(transform.forward * currentPropulsion, ForceMode.Acceleration);

        if(decayActive == true)
        {
            currentPropulsion = 0;
            decayActive = false;
        }
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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - shipCamera.position), shipRotLerpSpeed * Time.deltaTime);
    }
}