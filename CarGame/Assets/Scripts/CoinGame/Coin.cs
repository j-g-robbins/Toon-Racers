using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI coinText;

    void Awake() {
        coinText.text = "Coins: 0";
    }

    private void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
        coinText.text = "Coins: " + CoinManager.instance.lastCoinNum.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.lastCoinNum += 1;
            Destroy(gameObject);
        }
    }
}
