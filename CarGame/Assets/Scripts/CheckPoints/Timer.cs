using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public TextMeshProUGUI checkpointText;
    public GameObject restartText; 
    private double startTime; 
    private bool start = false;
    private bool finish = false; 

    // Splits
    private double splitTime;
    private double splitDifference;
    private double[] checkpointSplits = new double[21];
    //[SerializeField] public TextMeshProUGUI splitTimeText;
    [SerializeField] public TextMeshProUGUI splitDifText;

    // Start is called before the first frame update
    void Start() {
        restartText.SetActive(false); 
        timerText.text = "0:00.00";
        checkpointText.text = "1 / " + CheckPointManager.instance.numCheckpoints;
    }

    // Update is called once per frame
    void Update() {

        if (finish) {
            return; 
        }

        if (start) {
            // time since timer starts 
            double t = Time.time - startTime; 

            if (t > 0) {
                string minutes = ((int) t / 60).ToString(); 
                string seconds = secondsTime(t);
                checkpointText.text = CheckPointManager.instance.lastCheckPointNum + " / " + CheckPointManager.instance.numCheckpoints + "\n";
                timerText.text = minutes + ":" + seconds ;
                 
            }
        }
    }

    public void crossLine() {
        if (!start) {
            start = true;
            startTime = Time.time + 3.0f;
        } 
        else if (start) {
            finish = true;
            timerText.color = Color.red;
            checkpointText.color = Color.red;
            float t = (float)(Time.time - startTime);
            restartText.SetActive(true); 
            if (!PlayerPrefs.HasKey("FastestTime")) {
                SaveScore(t);
            } else if (t < PlayerPrefs.GetFloat("FastestTime")) {
                SaveScore(t);
            }
        }
    }

    public void SaveScore(float time) {
        PlayerPrefs.SetFloat("FastestTime", time);
        for (int i = 1; i < CheckPointManager.instance.numCheckpoints; i++) {
            PlayerPrefs.SetFloat("CheckPoint" + i, (float)checkpointSplits[i]);
        }
        PlayerPrefs.Save();
    }

    

    public void Split(int checkpointNum) {
        string minutes, seconds;
        splitTime = Time.time - startTime;
        if (CheckPointManager.instance.lastCheckPointNum == 1 && checkpointNum == 21) {
            return;
        }
        else if (checkpointNum != FinishLine.checkpointNum) {
            // Display current split time
            //minutes = ((int) splitTime / 60).ToString(); 
            //seconds = secondsTime(splitTime);
            //splitTimeText.text = minutes + ":" + seconds;

            // Splits tracking
            checkpointSplits[checkpointNum] = splitTime;
            if (!PlayerPrefs.HasKey("FastestTime")) {
                return;
            }
            // Display split difference
            splitDifference = splitTime - PlayerPrefs.GetFloat("CheckPoint"+checkpointNum);
        } else {
            if (!PlayerPrefs.HasKey("FastestTime")) {
                return;
            }
            //splitTimeText.text = "";
            splitDifference = splitTime - PlayerPrefs.GetFloat("FastestTime");
        }

        if (splitDifference <= 0) {
            splitDifText.text = "-";
            splitDifText.color = Color.blue;
        } else {
            splitDifText.text = "+";
            splitDifText.color = Color.red;
        }
        minutes = ((int) splitDifference / 60).ToString(); 
        if ((splitDifference % 60) < 10) {
            seconds = "0" + Mathf.Abs((float)splitDifference % 60).ToString("f2");
        } else {
            seconds = Mathf.Abs((float)splitDifference % 60).ToString("f2");
        }
        splitDifText.text = splitDifText.text + minutes + ":" + seconds;
    } 

    public static string secondsTime(double t) {
        if ((t % 60) < 10) {
            return "0" + (t % 60).ToString("f2");
        }
        return (t % 60).ToString("f2");
    }
}