using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHeight : MonoBehaviour
{
    public static float defaultCameraHeight = 3.5f;
    public Slider heightSlider;

    public void Awake() 
    {
        if (!PlayerPrefs.HasKey("CameraHeight")) {
            PlayerPrefs.SetFloat("CameraHeight", defaultCameraHeight);
        }
        heightSlider.value = PlayerPrefs.GetFloat("CameraHeight");
        PlayerPrefs.Save();

    }

    public void Height(float sliderValue) 
    {
        PlayerPrefs.SetFloat("CameraHeight", sliderValue);
        PlayerPrefs.Save();
    }
    
}
