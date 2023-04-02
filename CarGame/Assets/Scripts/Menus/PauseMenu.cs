using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public static bool Paused = false;
    public static bool firstPerson = false;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject settingsPauseMenu;
    [SerializeField] public GameObject newCamera;

    void Start() {
        Paused = false;
        if (firstPerson) {
            firstPerson = !firstPerson;
            SwitchCamera();
        }
        pauseMenu.SetActive(false);
        settingsPauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) & !Paused) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.C) & !Paused)
        {
            SwitchCamera();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (Paused) 
            {
                if (settingsPauseMenu.activeSelf) {
                    BackToPauseMenu();
                } 
                else 
                {
                    Resume();
                }
            } else 
            {
                Pause();
            }
        }
    }

    void Pause() 
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        Paused = true;
    }

    public void Resume() 
    {
        pauseMenu.SetActive(false);
        settingsPauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void Settings() {
        pauseMenu.SetActive(false);
        settingsPauseMenu.SetActive(true);
    }

    public void BackToPauseMenu() {
        settingsPauseMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Reset() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SwitchCamera() {
        firstPerson = !firstPerson;
        newCamera.GetComponent<FirstPerson>().enabled = !newCamera.GetComponent<FirstPerson>().enabled;
        newCamera.GetComponent<ThirdPerson>().enabled = !newCamera.GetComponent<ThirdPerson>().enabled;
    }
}
