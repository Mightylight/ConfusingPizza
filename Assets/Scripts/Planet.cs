using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class Planet : MonoBehaviour
{
    //TODO Make the End planet implementation
    public string planetName; 
    public bool isExplored;
    
    [SerializeField] private Topping _topping;

    
    
    private bool _isEndPlanet;

    private void Start()
    {
        planetName = gameObject.name;
    }
    
    public void AddTopping(Topping pTopping)
    {
        _topping = pTopping;
    }
    
    public void SetEndPlanet()
    {
        _isEndPlanet = true;
    }

    public Topping GetTopping()
    {
        return _topping;
    }

    //OnTriggerEnter is also a possibility
    private void OnCollisionEnter(Collision pOther)
    {
        if (!pOther.gameObject.CompareTag("Player") || isExplored) return;
        if (!_isEndPlanet)
        {
            GameManager.Instance.StartQTE(this);
        }
        else
        {
            //TODO: UI of quest complete
            GameManager.Instance.EndPlanet();
        }
    }
}


