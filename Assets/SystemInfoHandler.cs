using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemInfoHandler : MonoBehaviour {

    public Planets.System sysInfo { get; set; }

    public GameObject systemInfoPrefab;
    public GameObject planetInfoPrefab;
    public GameObject planetInfoEntryPrefab;

    bool alreadyGenerated = false;

    private void Awake() {
        systemInfoPrefab = Planets.instance.starInfoPrefab;
        planetInfoPrefab = Planets.instance.planetInfoPrefab;
        planetInfoEntryPrefab = Planets.instance.planetInfoEntryPrefab;
    }

    public void GenerateUi() {
        if (alreadyGenerated) return;

        /* STAR UI part */

        var starui = Instantiate(systemInfoPrefab, transform.parent, true);
        var rend = transform.parent.transform.GetChild(1).GetComponent<Renderer>();
        Vector3 center = rend.bounds.center;
        float radius = rend.bounds.extents.magnitude;
        Vector3 parPos = transform.parent.transform.GetChild(1).transform.position;
        starui.transform.position = parPos + new Vector3(-radius/2 - 3f, 0,0);

        var holder = starui.transform.GetChild(0).transform;


        holder.GetChild(1).GetComponent<Text>().text = 
            sysInfo.star.name;

        if (sysInfo.distance != null && sysInfo.distance != "") {
            holder.GetChild(2).GetComponent<Text>().text =
    "Distance to Sol: " + (float.Parse(sysInfo.distance) * 3.26f).ToString() + " LY";
        } else {
            holder.GetChild(2).GetComponent<Text>().text =
    "Distance to Sol: N/A";
        }

        if (sysInfo.star.mass != null && sysInfo.star.mass != "") {
            holder.GetChild(3).GetComponent<Text>().text = "Mass: " + sysInfo.star.mass + " Suns";
        } else {
            holder.GetChild(3).GetComponent<Text>().text = "Mass: N/A";
        }

        if (sysInfo.star.temperature != null && sysInfo.star.temperature != "") {
            holder.GetChild(4).GetComponent<Text>().text = "Temperature: " + sysInfo.star.temperature + "K";
        } else {
            holder.GetChild(4).GetComponent<Text>().text = "Temperature: N/A";
        }

        if (sysInfo.star.spectraltype != null && sysInfo.star.spectraltype != "") {
            holder.GetChild(5).GetComponent<Text>().text = "Spectral type: " + sysInfo.star.spectraltype;
        } else {
            holder.GetChild(5).GetComponent<Text>().text = "Spectral type: N/A";
        }

        alreadyGenerated = true;

        /*PLANET UI part*/


        var planetui = Instantiate(planetInfoPrefab, transform.parent, true);
        planetui.transform.position = parPos + new Vector3(radius / 2 + 5f, 0, 0);

        
        for (int i = 0; i < sysInfo.planets.Count; i++) {

            var planetEntry = Instantiate(planetInfoEntryPrefab, planetui.transform.GetChild(1).transform, true);
            var pholder = planetEntry.transform.GetChild(0).transform;

            pholder.GetChild(0).GetComponent<Text>().text = sysInfo.planets[i].name;

            string mass = sysInfo.planets[i].mass;
            if (mass != null && mass != "")
                pholder.GetChild(1).GetComponent<Text>().text = mass;
            else
                pholder.GetChild(1).GetComponent<Text>().text = "N/A";

            string diam = (sysInfo.planets[i].nRadius * 2).ToString();
            if (diam != null && diam != "")
                pholder.GetChild(2).GetComponent<Text>().text = diam;
            else
                pholder.GetChild(2).GetComponent<Text>().text = "N/A";

            string yod = sysInfo.planets[i].discoveryyear;
            if (yod != null && yod != "")
                pholder.GetChild(3).GetComponent<Text>().text = yod;
            else
                pholder.GetChild(3).GetComponent<Text>().text = "N/A";

            string mod = sysInfo.planets[i].discoverymethod;
            if (mod != null && mod != "")
                pholder.GetChild(4).GetComponent<Text>().text = mod;
            else
                pholder.GetChild(4).GetComponent<Text>().text = "N/A";

            planetEntry.transform.localScale = new Vector3(1, 1, 1);
            planetEntry.transform.localPosition = new Vector3(0, 0, 0);
        }

        //var holder = starui.transform.GetChild(0).transform;


    }
}
