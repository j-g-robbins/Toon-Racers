using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject musicVolume;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (musicVolume == null) {
            musicVolume = this.gameObject;
        } else {
            GameObject.Destroy(this.gameObject);
            Debug.Log("here");
        }
        
    }
}
