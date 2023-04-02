using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class RestartPrompt : MonoBehaviour
{

    public GameObject restartText; 

    void Start() {
        restartText.SetActive(false); 
    }
        
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Finish") {
            restartText.SetActive(true); 
        }
    }

    void update() {

    }
}
