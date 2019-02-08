using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarStarLaserDetection : MonoBehaviour {

    public void Go() {
        string systemName = transform.name.Substring(0, transform.name.Length - 4);
        Debug.Log("System " + systemName + " trig-clicked on Far-Star view.");
        Planets.instance.Add2dSystem(Planets.instance.FindBySystemName(systemName));
    }
}
