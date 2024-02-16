using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _timeLimitInSeconds;
    [SerializeField] private int _amountOfToppingsPerOrder;
    [SerializeField] private List<Topping> _toppings;
    [SerializeField] private List<Planet> _planets;
    [SerializeField] private Sprite _endplanetindicator;
    
    
    [Header("Gameplay")]
    public int points = 0;

    public List<Topping> aqquiredToppings = new();
    [SerializeField] private Order _currentOrder;

    [Header("Misc")]
    private Planet _endPlanet;
    public int[] savedScores = {0, 0, 0, 0, 0};
    
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _toppingText;
    
    [SerializeField] private GameObject _planetParent;
    private float _startTime;
    
    [SerializeField] private QuickTimeEvent _quickTimeEvent;
    [SerializeField] private GameObject _QTECanvas;
    [SerializeField] private GameObject _endPlanetCanvas;
    [SerializeField] private GameObject _endRequirementNotMetCanvas;
    
    private bool stopTimer = false;
    
    
    
    public static GameManager Instance;
    private void Awake()
    {
        DontDestroyOnLoad(this);
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
        Debug.Log("Got random order");
        DecoratePlanets();
        Debug.Log("Decorated Planets");
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

    private bool CheckForCompletion()
    {
        List<Topping> orderList = _currentOrder.GetToppings();
        foreach (Topping topping in orderList)
        {
            if(aqquiredToppings.Contains(topping))
            {
                
            }
            else
            {
                return false;
            }
        }
        Debug.Log("Order Completed");
        return true;
    }

    private void DecoratePlanets()
    {
        //Get a random planet and make it the deliver planet
        List<Planet> planets = new List<Planet>(_planets);
        int randomIndex = UnityEngine.Random.Range(0, planets.Count);
        _endPlanet = planets[randomIndex];
        _endPlanet.SetEndPlanet();
        planets.RemoveAt(randomIndex);
        Debug.Log(_endPlanet.name);
        Topping indicator = new Topping();
        indicator.toppingSprite = _endplanetindicator;
        _endPlanet.AddTopping(indicator);
        List<Topping> toppings = _currentOrder.GetToppings();
        

        if(planets.Count < toppings.Count)
        {
            Debug.LogError("Not enough planets to decorate");
            return;
        }
        
        // Add toppings to the remaining planets
        foreach (Topping topping in _toppings)
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
        //_planets.Remove(_endPlanet);
    }

    private void UpdateTimer()
    {
        if(stopTimer) return;
        if (Time.time - _startTime > _timeLimitInSeconds)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Game over");
        }
        //Display time in minutes
        float timeLeft = _timeLimitInSeconds - (Time.time - _startTime);
        int minutes = Mathf.FloorToInt(timeLeft / 60F);
        int seconds = Mathf.FloorToInt(timeLeft - minutes * 60);
        string fancyTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        _timerText.text = fancyTime;
    }
    
    private void GetRandomOrder()
    {
        Order order = new();
        List<Topping> tempToppingsList = new List<Topping>(_toppings);
        for (int i = 0; i < _amountOfToppingsPerOrder; i++)
        {
            Topping topping = tempToppingsList[UnityEngine.Random.Range(0, tempToppingsList.Count)];
            order.AddTopping(topping);
            tempToppingsList.Remove(topping);
        }
        _currentOrder = order;
        List<Topping> toppings = _currentOrder.GetToppings();
        string ingredientString = "";
        for (int i = 0 ; i < toppings.Count ; i++)
        {
            ingredientString += $"{i + 1}. {toppings[i].toppingName}\n";
        }
        _toppingText.text = ingredientString;
    }

    public void EndPlanet()
    {
        if (CheckForCompletion())
        {
            //End the game, you've won
            //_endPlanetCanvas.SetActive(true);
            stopTimer = true;
            float timeLeft = _timeLimitInSeconds - (Time.time - _startTime);
            points += Mathf.FloorToInt(timeLeft);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            //Show player that they do not have everything yet
            _endRequirementNotMetCanvas.SetActive(true);
        }
    }

    public void StartQTE(Planet pPlanet)
    {
        CursorManager.CursorState(false);
        ShipCameraController.CameraLock(true);
        _QTECanvas.SetActive(true);
        _quickTimeEvent.StartEvent(pPlanet);
    }
    
    public void EndQTE()
    {
        _QTECanvas.SetActive(false);
        ShipCameraController.CameraLock(false);
        CursorManager.CursorState(true);
    }
    
    public void AddPoints(int pPoints)
    {
        points += pPoints;
    }
    
    public void AddTopping(Topping pTopping)
    {
        aqquiredToppings.Add(pTopping);
        UpdateOrderText();
    }

    private void UpdateOrderText()
    {
        List<Topping> toppings = _currentOrder.GetToppings();
        string ingredientString = "";
        for (int i = 0; i < toppings.Count; i++)
        {
            if(aqquiredToppings.Contains(toppings[i]))
            {
                ingredientString += $"<s>{i + 1}. {toppings[i].toppingName} </s>\n";
            }
            else
            {
                ingredientString += $"{i + 1}. {toppings[i].toppingName}\n";
            }
        }
        _toppingText.text = ingredientString;
    }
}
