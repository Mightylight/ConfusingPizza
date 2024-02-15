using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _timeLimitInSeconds;
    [SerializeField] private int _amountOfToppingsPerOrder;
    [SerializeField] private List<Topping> _toppings;
    [SerializeField] private List<Planet> _planets;
    
    [Header("Gameplay")]
    public int points = 0;

    public List<Topping> aqquiredToppings = new();
    [SerializeField] private Order _currentOrder;

    [Header("Misc")]
    private Planet _endPlanet;
    
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GameObject _planetParent;
    private float _startTime;
    
    [SerializeField] private QuickTimeEvent _quickTimeEvent;
    [SerializeField] private GameObject _QTECanvas;
    
    
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _startTime = Time.time;
        GetRandomOrder();
        DecoratePlanets();
    }

    private void Update()
    {
        UpdateTimer();
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            _quickTimeEvent.StartEvent(_planets[0]);
            _QTECanvas.SetActive(true);
        }
    }

    private void DecoratePlanets()
    {
        List<Planet> planets = new List<Planet>(_planets);
        int randomIndex = UnityEngine.Random.Range(0, planets.Count);
        _endPlanet = planets[randomIndex];
        planets.RemoveAt(randomIndex);
        _planets.Remove(_endPlanet);
        List<Topping> toppings = _currentOrder.GetToppings();
        

        if(planets.Count <= toppings.Count)
        {
            Debug.LogError("Not enough planets to decorate");
            return;
        }
        // Add toppings to the planets
        foreach (Topping topping in toppings)
        {
            randomIndex = UnityEngine.Random.Range(0, planets.Count);
            Debug.Log(randomIndex);
            planets[randomIndex].AddTopping(topping);
            planets.RemoveAt(randomIndex);
        }
        
        //Fill the remaining planets with random toppings
        foreach (Planet planet in planets)
        {
            randomIndex = UnityEngine.Random.Range(0, _toppings.Count);
            planet.AddTopping(_toppings[randomIndex]);
        }
    }

    private void UpdateTimer()
    {
        if (Time.time - _startTime > _timeLimitInSeconds)
        {
            // End the game
            // TODO: Highscore system
        }
        
        float timeLeft = _timeLimitInSeconds - (Time.time - _startTime);
        _timerText.text = $"Time: {timeLeft.ToString("F0")}";
    }

    private void GetRandomOrder()
    {
        Order order = new();
        for (int i = 0; i < _amountOfToppingsPerOrder; i++)
        {
            order.AddTopping(_toppings[UnityEngine.Random.Range(0, _toppings.Count)]);
        }
        _currentOrder = order;
    }
    
    public void StartQTE(Planet pPlanet)
    {
        _QTECanvas.SetActive(true);
        _quickTimeEvent.StartEvent(pPlanet);
    }
    
    public void EndQTE()
    {
        _QTECanvas.SetActive(false);
    }
    
    public void AddPoints(int pPoints)
    {
        points += pPoints;
    }
    
    public void AddTopping(Topping pTopping)
    {
        aqquiredToppings.Add(pTopping);
    }
}
