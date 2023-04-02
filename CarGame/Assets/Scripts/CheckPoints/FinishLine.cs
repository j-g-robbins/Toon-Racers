using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class FinishLine : MonoBehaviour
{
    public GameObject car;
    private Timer _TimerScript;
    public static int checkpointNum = 21;
    private bool firstPass = true;
        
    private void OnTriggerEnter(Collider other) {
        if ((CheckPointManager.instance.lastCheckPointNum == checkpointNum) ||
        (CheckPointManager.instance.lastCheckPointNum == 0 && firstPass) ) {
            firstPass = false;
            CheckPointManager.instance.lastCheckPointNum += 1;
            _TimerScript = car.GetComponent<Timer>();
            _TimerScript.crossLine();
            CheckPointManager.instance.lastCheckPointPos = this.transform.position;
            CheckPointManager.instance.lastCheckPointRot = this.transform.rotation;
            _TimerScript.Split(checkpointNum);
            //RestartPrompt.restartText.SetActive(true); 
        }
    }
}
