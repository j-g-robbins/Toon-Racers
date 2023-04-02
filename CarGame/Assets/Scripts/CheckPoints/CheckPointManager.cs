using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;
    public Vector3 lastCheckPointPos;
    public Quaternion lastCheckPointRot;
    public int numCheckpoints = 21;
    public int lastCheckPointNum = 0;

    public Timer _TimerScript;
    public GameObject car;

    public void Awake()
    {
        instance = this;
        _TimerScript = car.GetComponent<Timer>();
    }

}
