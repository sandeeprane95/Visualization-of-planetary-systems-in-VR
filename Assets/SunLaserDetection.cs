using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLaserDetection : MonoBehaviour {

    public void RemoveSystem()
    {
        Transform parent = gameObject.transform.parent;
        string systemName = parent.name.Substring(10);
        Debug.Log("System's " + systemName + " star clicked on 3d view");
        Planets.instance.Remove3dSystem(systemName);
    }
    public void SpawnUiInfo() {
        gameObject.SendMessage("GenerateUi");
    }
}
