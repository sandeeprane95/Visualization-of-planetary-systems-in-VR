using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SimOptions
{
    public float oScale;
    public float revScale;
    public float pScale;
}

public class SimOptionsManager : MonoBehaviour {

    float oScale;
    float revScale;
    float pScale;

    static public SimOptionsManager instance;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	public void UpdateOrbitScale(float val)
    {
        oScale = val;
    }

    public void UpdatePlanetScale(float val)
    {
        pScale = val;
    }

    public void UpdateRevolutionSpeed(float val)
    {
        revScale = val;
    }

    public void AbandonSettings()
    {
        oScale = 1;
        revScale = 1;
        pScale = 1;
    }

    public void ApplyNewSettings()
    {
        SimOptions o = new SimOptions { oScale = this.oScale, pScale = this.pScale, revScale = this.revScale };
        SystemBuilder.ChangeOptions(o);
        Planets.instance.RegenerateViewsWithNewOptions();
    }}
