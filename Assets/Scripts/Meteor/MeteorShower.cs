using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorShower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShipMovementController shipMovement;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject meteorPrefab;

    [Header("Meteor Values")]
    [SerializeField] private float pushForce;
    [Space]
    [SerializeField] private float spawnDistance;
    [Space]
    [SerializeField] private float spawnTime;
    [SerializeField] private float minSpawnTime, maxSpawnTime;
    private float spawnTimePassed;
    [Space]
    [SerializeField] private float meteorSpeed;
    [SerializeField] private float minMeteorSpeed, maxMeteorSpeed;
    [Space]
    [SerializeField] private float meteorRotSpeed;
    [SerializeField] private float minMeteorRotSpeed, maxMeteorRotSpeed;
    [Space]
    [SerializeField] private Vector3 meteorScale;
    [SerializeField] private float minMeteorScale, maxMeteorScale;
    private Vector3 spawnPos;
    private Vector3 randomDir;

    [Header("Aiming")]
    public Vector3 currentVelocity;
    public Vector3 targetPrevPos;
    public Vector3 aimPos;

    private void Start()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void Update()
    {
        TargetVelocity();

        if(spawnTimePassed >= spawnTime)
        {
            SpawnMeteor();
            spawnTimePassed = 0;
        }

        spawnTimePassed += Time.deltaTime;
    }

    private void RandomMeteorValues()
    {
        randomDir = Random.onUnitSphere.normalized;
        randomDir *= spawnDistance;

        meteorSpeed = Random.Range(minMeteorSpeed, maxMeteorSpeed);
        meteorRotSpeed = Random.Range(minMeteorRotSpeed, maxMeteorRotSpeed);

        float randomScale = Random.Range(minMeteorScale, maxMeteorScale);
        meteorScale = Vector3.one * randomScale;
    }

    private void TargetVelocity()
    {
        //currentVelocity = (target.position - targetPrevPos) / Time.deltaTime;
        //targetPrevPos = target.position;

        currentVelocity = shipMovement.rb.velocity;
    }

    private void SpawnMeteor()
    {
        RandomMeteorValues();

        spawnPos = target.position + randomDir;

        float targetDistance = Vector3.Distance(spawnPos, target.position);
        float travelTime = targetDistance / meteorSpeed;

        aimPos = target.position + currentVelocity * travelTime;

        GameObject meteorObject = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        meteorObject.transform.rotation = Quaternion.LookRotation(aimPos - meteorObject.transform.position);

        if(meteorObject.TryGetComponent<Meteor>(out Meteor meteor))
        {
            meteor.SetMeteor(meteorSpeed, meteorRotSpeed, meteorScale, pushForce);
        }

        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(target.position, spawnDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(spawnPos, Vector3.one * 10);
        Gizmos.DrawCube(aimPos, Vector3.one * 10);
    }
}