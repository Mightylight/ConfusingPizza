using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("")]
    [SerializeField] private Transform meteorMesh;

    [Header("Meteor Values")]
    [SerializeField] private float speed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float lifeTime;
    private float pushForce;
    private float currentLifeTime;
    private Vector3 lastPos;

    private void Update()
    {
        meteorMesh.Rotate(rotSpeed * Time.deltaTime * Vector3.one);
        transform.Translate(speed * Time.deltaTime * Vector3.forward);

        if(Physics.Linecast(lastPos, transform.position, out RaycastHit hit))
        {
            if(hit.transform.TryGetComponent<ShipMovementController>(out ShipMovementController shipMovement))
            {
                shipMovement.Hit(transform.forward, pushForce);
                Destroy(gameObject);
            }
        }

        if(currentLifeTime >= lifeTime)
        {
            Destroy(gameObject);
        }

        currentLifeTime += Time.deltaTime;
        lastPos = transform.position;
    }

    public void SetMeteor(float speed, float rotSpeed, Vector3 scale, float pushForce)
    {
        this.speed = speed;
        this.rotSpeed = rotSpeed;
        transform.localScale = scale;
        this.pushForce = pushForce;
    }
}