using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject settings;
    [SerializeField] public GameObject controls;
    [SerializeField] public GameObject gameSelect;
    
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        
        settings.SetActive(true);
        Update();
        settings.SetActive(false);

        controls.SetActive(false);
        gameSelect.SetActive(false);
        
        // if (!PlayerPrefs.HasKey("FirstOpen")) {
        //     PlayerPrefs.DeleteAll();
        //     PlayerPrefs.SetInt("FirstOpen", 0);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainMenu.activeSelf & Input.GetKeyDown(KeyCode.Escape)) 
        {
            Back();
        }
    }

    public void Settings() {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void Controls() {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void GameSelect() {
        mainMenu.SetActive(false);
        gameSelect.SetActive(true);
    }

    public void Back() {
        if (settings.activeSelf) {
            settings.SetActive(false);
        } else if (controls.activeSelf) {
            controls.SetActive(false);
        } else if (gameSelect.activeSelf) {
            gameSelect.SetActive(false);
        }
        mainMenu.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void StartRace()
    {
        gameSelect.SetActive(false);
        SceneManager.LoadSceneAsync(1);
    }

    public void StartCoin()
    {
        gameSelect.SetActive(false);
        SceneManager.LoadSceneAsync(2);
    }
}
