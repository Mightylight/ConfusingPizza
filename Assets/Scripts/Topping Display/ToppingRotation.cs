using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingRotation : MonoBehaviour
{
    public float spinSpeed = 50f;
    public float bounceHeight = 0.1f;
    public float bounceFrequency = 1f;

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the object
        initialPosition = transform.position;
    }

    void Update()
    {
        // Rotate the GameObject around its up axis (Y-axis) at a constant speed
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

        // Apply bouncing effect
        Vector3 newPosition = initialPosition;
        newPosition.y += Mathf.Sin(Time.time * bounceFrequency) * bounceHeight;
        transform.position = newPosition;
    }
}
