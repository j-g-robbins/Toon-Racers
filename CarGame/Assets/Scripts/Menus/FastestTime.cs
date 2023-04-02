using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FastestTime : MonoBehaviour
{
    public TextMeshProUGUI fastestTimeText;
    private float time;

    void Awake() {
        if (PlayerPrefs.HasKey("FastestTime")) {
            time = PlayerPrefs.GetFloat("FastestTime");
            string minutes = ((int) time / 60).ToString(); 
            string seconds = Timer.secondsTime(time);
            fastestTimeText.text = "BEST TIME: " + minutes + ":" + seconds;
        }
        PlayerPrefs.Save();
    }
}
