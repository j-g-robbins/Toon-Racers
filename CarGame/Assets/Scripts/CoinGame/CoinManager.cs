using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager: MonoBehaviour
{
    public static CoinManager instance;
    public int TotalCoins = 100;
    public int lastCoinNum = 0;

    public Timer _TimerScript;
    public GameObject car;

    public void Awake()
    {
        instance = this;
        _TimerScript = car.GetComponent<Timer>();
    }

}
