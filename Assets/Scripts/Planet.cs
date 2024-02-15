using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string planetName;
    [SerializeField] private Topping _topping;

    [SerializeField] private QuickTimeEvent _quickTimeEvent;
    
    
    private void OnCollisionEnter(Collision pOther)
    {
        if (pOther.gameObject.CompareTag("Player"))
        {
            // Start interaction with planet
            // QuickTimeEvent?
        }
    }
    
    private void OnCollisionExit(Collision pOther)
    {
        if (pOther.gameObject.CompareTag("Player"))
        {
            // End interaction with planet
            // QuickTimeEvent?
        }
    }
    
}


