using System;
using System.Collections;
using System.Collections.Generic;
using PsychoUnity.Manager;
using PsychoUnity.Peripheral.Pump;
using UnityEngine;

public class TestLabK1 : MonoBehaviour
{
    public string portName;
    private LabK1 _pump;

    private bool _isRewarding;
    void Start()
    {
        _pump = new LabK1(portName);
    }

    IEnumerator Reward()
    {
        if (_isRewarding) yield break;
        
        _isRewarding = true;
        _pump.GiveReward();

        yield return new WaitForSeconds(4);
            
        _pump.StopReward();
        _isRewarding = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Reward());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _pump.SetSpeed(150);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _pump.SetSpeed(50);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(_pump.GetSpeed());
        }
    }

    private void OnDestroy()
    {
        SerialComManager.Instance.Clear();
    }
}