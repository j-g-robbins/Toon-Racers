using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public static float defaultMusicVol = 0.5f;
    public Slider musicSlider;

    public void Awake () {
        if (!PlayerPrefs.HasKey("MusicVolume")) {
            PlayerPrefs.SetFloat("MusicVolume", defaultMusicVol);
        }
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetLevel(PlayerPrefs.GetFloat("MusicVolume"));
        PlayerPrefs.Save();
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        PlayerPrefs.Save();
    }
}
