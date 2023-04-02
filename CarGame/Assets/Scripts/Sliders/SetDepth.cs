using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetDepth : MonoBehaviour
{
    public static float defaultCameraDepth = 12f;
    public Slider depthSlider;

    public void Awake() 
    {
        if (!PlayerPrefs.HasKey("CameraDepth")) {
            PlayerPrefs.SetFloat("CameraDepth", defaultCameraDepth);
        }
        depthSlider.value = PlayerPrefs.GetFloat("CameraDepth");
        PlayerPrefs.Save();

    }

    public void Depth(float sliderValue) 
    {
        PlayerPrefs.SetFloat("CameraDepth", sliderValue);
        PlayerPrefs.Save();
    }
}
