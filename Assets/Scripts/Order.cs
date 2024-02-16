using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order
{
    [SerializeField] private List<Topping> _toppings = new();

    public void AddTopping(Topping pTopping)
    {
        // Add topping to the array
        _toppings.Add(pTopping);
    }

    public void RemoveTopping(Topping pTopping)
    {
        _toppings.Remove(pTopping);
    }
    
    public bool ContainsTopping(Topping pTopping)
    {
        return _toppings.Contains(pTopping);
    }

    public List<Topping> GetToppings()
    {
        // Return the order
        return _toppings;
    }
}
