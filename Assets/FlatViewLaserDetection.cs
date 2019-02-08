using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatViewLaserDetection : MonoBehaviour {

    public void Go()
    {
        Transform parent = gameObject.transform.parent;
        string systemName = parent.name.Substring(13);
        Debug.Log("System " + systemName + " trig-clicked on 2d view");
        Planets.instance.Add3dSystem(Planets.instance.FindBySystemName(systemName));
    }

    public void Delete()
    {
        Transform parent = gameObject.transform.parent;
        string systemName = parent.name.Substring(13);
        Debug.Log("System " + systemName + " pad-clicked on 2d view");
        Planets.instance.Remove2dSystem(systemName);
    }
}
