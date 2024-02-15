using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string _planetName; 
    [SerializeField] private Topping _topping;

    private void Start()
    {
        _planetName = gameObject.name;
    }
    
    public void AddTopping(Topping pTopping)
    {
        _topping = pTopping;
    }

    public Topping GetTopping()
    {
        return _topping;
    }

    //OnTriggerEnter is also a possibility
    private void OnCollisionEnter(Collision pOther)
    {
        if (!pOther.gameObject.CompareTag("Player")) return;
        
        // Start interaction with planet
        // Enable Canvas
        GameManager.Instance.StartQTE(this);
    }
}


