using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent : MonoBehaviour
{
    private int _points;
    private float _precisionMultiplier;
    [SerializeField] private float _timeToPress = 1f;
    
    
    private bool _hasStarted = true;
    private bool _hasEnded = false;
    private float _currentValue;
    [SerializeField] private Slider _objectToMove;
    [SerializeField] private RectTransform[] _pressBoundryObjects;
    
    
    
    [SerializeField] private float speed;

    private void Start()
    {
        StartEvent();
    }

    // Start is called before the first frame update
    public void StartEvent()
    {
        _hasStarted = true;
        _hasEnded = false;
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
        _currentValue = Mathf.Sin(Time.time * speed);
        Debug.Log(_currentValue);
        _objectToMove.value = _currentValue;

        if (!_hasStarted && _hasEnded) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        
        if(_currentValue < _timeToPress && _currentValue > -_timeToPress)
        {
            Debug.LogWarning("You pressed the button at the right time");
            // awarded points = points * precisionMultiplier
            _hasEnded = true;
        }
        else
        {
            Debug.LogWarning("You pressed the button at the wrong time");
            // awarded points = points;
            _hasEnded = true;
        }
    }
}
