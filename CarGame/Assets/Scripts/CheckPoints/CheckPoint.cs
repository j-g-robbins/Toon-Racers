using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] public Material material1;
    [SerializeField] public GameObject checkpoint;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (int.Parse(this.name.Substring(12)) == CheckPointManager.instance.lastCheckPointNum) {
                CheckPointManager.instance._TimerScript.Split(CheckPointManager.instance.lastCheckPointNum);
                CheckPointManager.instance.lastCheckPointPos = this.transform.position;
                CheckPointManager.instance.lastCheckPointRot = this.transform.rotation;
                CheckPointManager.instance.lastCheckPointNum += 1;
                checkpoint.GetComponent<MeshRenderer>().material = material1;
            }
        }
    }
}
