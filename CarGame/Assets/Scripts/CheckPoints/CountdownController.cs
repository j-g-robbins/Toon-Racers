using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class CountdownController : MonoBehaviour
{
    public int countdownTime; 
    public TextMeshProUGUI countdownText; 

    private void Start() {
        StartCoroutine(CountdownStart()); 
    }

    IEnumerator CountdownStart() {
        while(countdownTime > 0) {
            countdownText.text = countdownTime.ToString(); 
            yield return new WaitForSeconds(1f); 

            countdownTime--; 
        }

        countdownText.text = "GO!"; 
        yield return new WaitForSeconds(1f); 
        countdownText.gameObject.SetActive(false); 
    }


}
