using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetListItem : MonoBehaviour {

    public Planets.System sys { get; set; }
    [SerializeField]
    Text planetsNo, distance, sysName;

	// Use this for initialization
	void Start () {
        Invoke("Setup", .05f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void Setup() {
        planetsNo.text = sys.planets.Count + " planets";
        sysName.text = sys.name;
        float lyears = float.Parse(sys.distance) * 3.26f;
        distance.text = lyears  + "LY away";
    }

    public void SpawnAssignedSystem() {
        Planets.instance.Add2dSystem(sys);
    }
}
