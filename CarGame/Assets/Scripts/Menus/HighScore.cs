using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Awake() {
        if (PlayerPrefs.HasKey("HighScore")) {
            highScoreText.text = "MOST COINS: " + PlayerPrefs.GetInt("HighScore").ToString();
        }
        PlayerPrefs.Save();
    }
}
