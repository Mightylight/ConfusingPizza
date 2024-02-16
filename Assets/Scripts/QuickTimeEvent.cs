using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent : MonoBehaviour
{
    
    [Range(0,1)]
    [SerializeField] private float _timeToPress = 1f;
    [SerializeField] private float _speed;
    [SerializeField] private int _precisionMultiplier;
    [SerializeField] private int _points;

    [SerializeField] private GameObject _toppingSprite;
    
    
    [SerializeField] private Slider _objectToMove;
    [SerializeField] private RectTransform[] _pressBoundryObjects;
    
    private GameManager _gameManager;
    private Planet _planet;
    private bool _hasStarted = true;
    private bool _hasEnded = false;
    private float _currentValue;

    // Start is called before the first frame update
    public void StartEvent(Planet pPlanet)
    {
        _gameManager = GameManager.Instance;
        _planet = pPlanet;
        _hasStarted = true;
        _hasEnded = false;
        _toppingSprite.SetActive(false);
        _objectToMove.gameObject.SetActive(true);
        CreateBorders();
    }

    private void CreateBorders()
    {
        _pressBoundryObjects[0].gameObject.SetActive(true);
        _pressBoundryObjects[0].localPosition = new Vector3(_timeToPress * 100f, 0, 0);
        _pressBoundryObjects[1].gameObject.SetActive(true);
        _pressBoundryObjects[1].localPosition = new Vector3(-_timeToPress * 100f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEvent();
    }

    private void UpdateEvent()
    {
        if (!_hasStarted || _hasEnded) return;
        _currentValue = Mathf.Sin(Time.time * _speed);
        //Debug.Log(_currentValue);
        _objectToMove.value = _currentValue;
    }

    public void TriggerEvent()
    {
        if (!_hasStarted || _hasEnded) return;
        int points = _points;
        if (_currentValue < _timeToPress && _currentValue > -_timeToPress)
        {
            Debug.LogWarning("You pressed the button at the right time");
            // awarded points = points * precisionMultiplier
            points *= _precisionMultiplier;
        }
        else
        {
            Debug.LogWarning("You pressed the button at the wrong time");
        }
        _gameManager.AddPoints(points);
        _planet.isExplored = true;
        _gameManager.AddTopping(_planet.GetTopping());
        _hasEnded = true;

        //Disable canvas with a coroutine
        StartCoroutine(DisableCanvas());
        _objectToMove.gameObject.SetActive(false);
        Topping topping = _planet.GetTopping();
        _toppingSprite.GetComponent<Image>().sprite = topping.toppingSprite;
        _toppingSprite.SetActive(true);
    }
    
    private IEnumerator DisableCanvas()
    {
        yield return new WaitForSeconds(5);
        GameManager.Instance.EndQTE();
    }
}
