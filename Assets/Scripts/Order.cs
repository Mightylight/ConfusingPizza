using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Order : ScriptableObject
{
    private List<Topping> _toppings;

    public void AddTopping(Topping pTopping)
    {
        // Add topping to the array
        _toppings.Add(pTopping);
    }

    public List<Topping> GetToppings()
    {
        // Return the order
        return _toppings;
    }
}
