using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class CoinTimer : MonoBehaviour
{

    public double timeRemaining = 93;

    [SerializeField] public TextMeshProUGUI timerText;

    public GameObject restartText; 
    public GameObject coinManager;
    private double startTime; 


    public bool timerIsRunning = false;


    // Start is called before the first frame update
    void Start() {
        timerIsRunning = true;
        restartText.SetActive(false); 
        timerText.text = "0:00.00";
    }

    // Update is called once per frame
    void Update() {

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                string seconds;
                string minutes; 
                timeRemaining -= Time.deltaTime;
                
                if (timeRemaining > 90) {
                    minutes = (90 / 60).ToString();
                    seconds = secondsTime(90);
                }
                else{
                    minutes = ((int)timeRemaining / 60).ToString();
                    seconds = secondsTime(timeRemaining);
                }
                
                timerText.text = minutes + ":" + seconds;
            }
            else
            {
                //Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                timerText.text = "Finish";
                restartText.SetActive(true);
                this.GetComponent<CarController>().forwardSpeed = 0;
                if (PlayerPrefs.GetInt("HighScore") < coinManager.GetComponent<CoinManager>().lastCoinNum) {
                    PlayerPrefs.SetInt("HighScore", coinManager.GetComponent<CoinManager>().lastCoinNum);
                }
                
            }
        }


    }

    public static string secondsTime(double t) {
        if ((t % 60) < 10) {
            return "0" + (t % 60).ToString("f2");
        }
        return (t % 60).ToString("f2");
    }
}