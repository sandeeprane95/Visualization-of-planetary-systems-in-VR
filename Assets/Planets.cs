/// Sample Code for CS 491 Virtual And Augmented Reality Course - Fall 2017
/// written by Andy Johnson
/// 
/// makes use of various textures from the celestia motherlode - http://www.celestiamotherlode.net/

using System.Collections.Generic;

using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections;

public class Planets : MonoBehaviour {

    public static Planets instance;

    SystemBuilder sb; 
    List<System> systemData = new List<System>();
    Dictionary<string, System> systemsDict;
    public List<System> filteredList;

    public List<System> largestSuns = new List<System>();
    public List<System> mostPlanets = new List<System>();
    public List<System> recentSystems = new List<System>();
    public List<System> brightestSuns = new List<System>();
    public List<System> favSystems = new List<System>();

    List<System> systemsNowIn3d = new List<System>();
    List<System> systemsNowIn2d = new List<System>();

	static GameObject master_001;
    Vector3 systemOffset;
    Vector3 sideOffset;
    Vector3 oneOffset;
    public GameObject allCenter;
    public GameObject allSide;

    public GameObject starInfoPrefab;
    public GameObject planetInfoPrefab;
    public GameObject planetInfoEntryPrefab;


    void Clear3dView() {
        GameObject.Destroy(allCenter);
        allCenter = new GameObject();
        allCenter.name = "all systems";
        
        sb.dealWithNewSystem(systemsDict["Sun"], allCenter);
    }

    void Clear2dView() {
        GameObject.Destroy(allSide);
        allSide = new GameObject();
        allSide.name = "all side views";
        
        sb.Create2dView(systemsDict["Sun"], allSide);
    }

    public void RegenerateViewsWithNewOptions() {
        Clear2dView();
        Clear3dView();
        StartCoroutine(Regenerate());

    }

    IEnumerator Regenerate() {
        Debug.Log("Regenerating systems.");
        for (int i = 0; i < systemsNowIn3d.Count; i++) {
            Debug.Log("System " + i);
            sb.dealWithNewSystem(systemsNowIn3d[i], allCenter);
            yield return new WaitForSeconds(.05f);
        }
        Debug.Log("Regenerating 2d systems.");
        FlatSlotManager.instance.FreeAllSlots();
        for (int i = 0; i < systemsNowIn2d.Count; i++) {
            Debug.Log("System " + i);
            sb.Create2dView(systemsNowIn2d[i], allSide);
            yield return new WaitForSeconds(.05f);
        }
    }

    public void Remove3dSystem(string name) {
        if (name == "Sun") return;

        foreach (Transform child in allCenter.transform) {
            if (child.name == "SolarCenter " + name) {
                Destroy(child.gameObject);
                for (int i = 0; i < systemsNowIn3d.Count; i++) {
                    if (systemsNowIn3d[i].name == name) {
                        systemsNowIn3d.RemoveAt(i);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void Remove2dSystem(string name) {
        if (name == "Sun") return;

        foreach (Transform child in allSide.transform)
        {
            if (child.name == "Side View of " + name)
            {
                Destroy(child.gameObject);
                for (int i = 0; i < systemsNowIn2d.Count; i++)
                {
                    if (systemsNowIn2d[i].name == name)
                    {
                        systemsNowIn2d.RemoveAt(i);
                        break;
                    }
                }
                break;
            }
        }
    }

    public void Add3dSystem(System sys) {

        if (SlotManager.instance != null)
        {
            if (SlotManager.instance.AreThereFreeSlots() == false)
            {
                Debug.Log("No more space for systems on the 3dView!");
                return;
            }
        }

        if (!systemsNowIn3d.Contains(sys) && sys.star.name != "Sun")
        {
            sb.dealWithNewSystem(sys, allCenter);
            systemsNowIn3d.Add(sys);
        }
    }

    public void Add2dSystem(System sys) {

        if (FlatSlotManager.instance != null)
        {
            if (FlatSlotManager.instance.AreThereFreeSlots() == false)
            {
                Debug.Log("No more space for systems on the 3dView!");
                return;
            }
        }

        if (!systemsNowIn2d.Contains(sys) && sys.star.name != "Sun")
        {
            sb.Create2dView(sys, allSide);
            systemsNowIn2d.Add(sys);
        }
    }

    public System FindBySystemName(string name)
    {
        return systemsDict[name];
    }


    Vector3 GetCurrentSystemOffset()
    {
        return new Vector3
            (systemOffset.x,
            systemOffset.y + ((systemsNowIn3d.Count+1) * -30.0f), 
            systemOffset.z);
    }

    Vector3 GetCurrentSideOffset()
    {
        return new Vector3(
            sideOffset.x,
            sideOffset.y + ((systemsNowIn2d.Count+1) * -30.0f),
            sideOffset.z);
    }

    private void Update() {

    }

    void Start () {
        instance = this;

        systemOffset = new Vector3(0, 0, 0);
        sideOffset = new Vector3(50, 50, 0);
        oneOffset = new Vector3(0, -30, 0);

        gameObject.AddComponent<SystemBuilder>();
        sb = gameObject.GetComponent<SystemBuilder>();

        allSide = new GameObject();

        // Check for saved data, parse new if file not found. 
        // Results stored in List<Systems> systemData class variable.
        if (!File.Exists(Application.dataPath + "//planetData.dat")) {
            SaveSystemsToFile();
        }
        LoadStoredSystems();
        CreateFilteredLists();

        systemsDict = new Dictionary<string, System>();

        for (int i = 0; i < systemData.Count; i++) {
            systemsDict.Add(systemData[i].name, systemData[i]);
        }
        Debug.Log("systemsDict has this many entires: " + systemsDict.Count);

        /*
        * OLD SYSTEM DATA
        */

        string[] sol = new string[5] { "695500", "Our Sun", "sol", "G2V", "1.0" };

        string[,] solPlanets = new string[8, 5] {
            {   "57910000",  "2440",    "0.24", "mercury", "mercury" },
            {  "108200000",  "6052",    "0.62", "venus",   "venus" },
            {  "149600000",  "6371",    "1.00", "earthmap", "earth" },
            {  "227900000",  "3400",    "1.88", "mars",     "mars" },
            {  "778500000", "69911",   "11.86", "jupiter", "jupiter" },
            { "1433000000", "58232",   "29.46", "saturn",   "saturn" },
            { "2877000000", "25362",   "84.01", "neptune", "uranus" },
            { "4503000000", "24622",  "164.80", "uranus", "neptune" }
        };


        string[] TauCeti = new string[5] { "556400", "Tau Ceti", "gstar", "G8.5V", "0.52" };

        string[] Gliese581 = new string[5] { "201750", "Gliese 581", "mstar", "M3V", "0.013" };

        allCenter = new GameObject();
        allCenter.name = "all systems";

        allSide = new GameObject();
        allSide.name = "all side views";
        // CreateSolarSystem
        Invoke("CreateSol", .3f);        

        filteredList = new List<System>();

        for (int i = 0; i < systemData.Count; i++) {
            if(systemData[i].planets.Count > 4 && systemData[i].star.name != "Sun") {
                filteredList.Add(systemData[i]);
            }
        }

        Debug.Log("<b>Filtered list entries: </b>" + filteredList.Count);



        //allCenter.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        //allCenter.transform.localScale *= 5;


        /*
         * BACKGROUND STARS RELATIVE POSITION CODE
         */

        float sunRad = float.Parse(sol[0]) * 1.5F / 695500.0F;
        GameObject SunSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SunSphere.name = "SunBall";
        SunSphere.transform.position = new Vector3(0, 80, 100);
        SunSphere.transform.localScale = new Vector3(sunRad, sunRad, sunRad);
        GameObject sunSphereText = new GameObject();         // This is the gameobject for the text which appears near each sun/star
        sunSphereText.name = "Star Name";
        sunSphereText.transform.position = new Vector3(-0.4F, 80F + sunRad, 100);
        sunSphereText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var sunSphereTextMesh = sunSphereText.AddComponent<TextMesh>();
        sunSphereTextMesh.text = "Sun";
        sunSphereTextMesh.fontSize = 50;

        float tauCRad = float.Parse(TauCeti[0]) * 1.5F / 695500.0F;
        float tauDist = 112630000000000.0F * 2.0F / 30860000000000.0F;
        GameObject TauCetiSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        TauCetiSphere.name = "TauBall";
        TauCetiSphere.transform.position = new Vector3(0 + tauDist, 80 + tauDist * (Convert.ToSingle(Math.PI * -15.0 / 180.0)), 100 + tauDist * (Convert.ToSingle(Math.PI * 1.0 * 15.0F / 180.0)));
        TauCetiSphere.transform.localScale = new Vector3(tauCRad, tauCRad, tauCRad);
        GameObject TauCetiSphereText = new GameObject();         // This is the gameobject for the text which appears near each sun/star
        TauCetiSphereText.name = "TauCetiText";
        TauCetiSphereText.transform.position = new Vector3(0 + tauDist - 0.7F, 80F + tauDist * (Convert.ToSingle(Math.PI * -15.0 / 180.0)) + tauCRad, 100 + tauDist * (Convert.ToSingle(Math.PI * 1.0 * 15.0F / 180.0)));
        TauCetiSphereText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var TauCetiSphereTextMesh = TauCetiSphereText.AddComponent<TextMesh>();
        TauCetiSphereTextMesh.text = "Tau Ceti";
        TauCetiSphereTextMesh.fontSize = 50;

        float GlieseRad = float.Parse(Gliese581[0]) * 1.5F / 695500.0F;
        float GlieseDist = 191620000000000.0F * 2.0F / 30860000000000.0F;
        GameObject Gliese581Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Gliese581Sphere.name = "GlieseBall";
        Gliese581Sphere.transform.position = new Vector3(0 + GlieseDist, 80 + GlieseDist * (Convert.ToSingle(Math.PI * -7.0 / 180.0)), 100 + GlieseDist * (Convert.ToSingle(Math.PI * 15.0 * 15.0F / 180.0)));
        Gliese581Sphere.transform.localScale = new Vector3(GlieseRad, GlieseRad, GlieseRad);
        GameObject Gliese581SphereText = new GameObject();         // This is the gameobject for the text which appears near each sun/star
        Gliese581SphereText.name = "Gliese581Text";
        Gliese581SphereText.transform.position = new Vector3(0 + GlieseDist - 0.7F, 80F + GlieseDist * (Convert.ToSingle(Math.PI * -7.0 / 180.0)) + 0.6F, 100 + GlieseDist * (Convert.ToSingle(Math.PI * 15.0 * 15.0F / 180.0)));
        Gliese581SphereText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var Gliese581SphereTextMesh = Gliese581SphereText.AddComponent<TextMesh>();
        Gliese581SphereTextMesh.text = "Gliese 581";
        Gliese581SphereTextMesh.fontSize = 30;


        // Hierarchy-mess-fixer-code:
        GameObject starContainer = new GameObject();
        starContainer.name = "Far Star Container";
        GameObject farStarObjects = new GameObject();
        farStarObjects.name = "Far Star Objects";
        GameObject farStarLabels = new GameObject();
        farStarLabels.name = "Far Star Labels";
        farStarLabels.transform.parent = starContainer.transform;
        farStarObjects.transform.parent = starContainer.transform;

        foreach (System sys in systemData) {
            if (sys.distance != null) {
                GameObject starSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                starSphere.name = sys.name + "Ball";

                // This is for laser detection:
                starSphere.AddComponent<SphereCollider>();
                starSphere.GetComponent<SphereCollider>().isTrigger = true;
                starSphere.AddComponent<FarStarLaserDetection>();


                float starRad = float.Parse(sys.star.radius) * 1.5F;
                float starDist = float.Parse(sys.distance) * 2.0F;
                String[] starRA = sys.rightascension.Split(' ');
                String[] starDec = sys.declination.Split(' ');
                starSphere.transform.position = new Vector3(0 + starDist, 80 + starDist * (Convert.ToSingle(Math.PI * float.Parse(starDec[0]) / 180.0)), 100 + starDist * (Convert.ToSingle(Math.PI * float.Parse(starRA[0]) * 15.0F / 180.0)));
                starSphere.transform.localScale = new Vector3(starRad, starRad, starRad);
                GameObject starSphereText = new GameObject();         // This is the gameobject for the text which appears near each sun/star
                starSphereText.name = sys.name + "Text";
                if (starRad < 1)
                    starSphereText.transform.position = new Vector3(0 + starDist - 0.7F, 80F + starDist * (Convert.ToSingle(Math.PI * float.Parse(starDec[0]) / 180.0)) + starRad + 0.2F, 100 + starDist * (Convert.ToSingle(Math.PI * float.Parse(starRA[0]) * 15.0F / 180.0)));
                else
                    starSphereText.transform.position = new Vector3(0 + starDist - 0.7F, 80F + starDist * (Convert.ToSingle(Math.PI * float.Parse(starDec[0]) / 180.0)) + starRad, 100 + starDist * (Convert.ToSingle(Math.PI * float.Parse(starRA[0]) * 15.0F / 180.0)));
                starSphereText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
                var starSphereTextMesh = starSphereText.AddComponent<TextMesh>();
                starSphereTextMesh.text = sys.name;
                if (sys.name.Length <= 15)
                    starSphereTextMesh.fontSize = 30;
                else
                    starSphereTextMesh.fontSize = 20;

                starSphere.transform.parent = farStarObjects.transform;
                starSphereText.transform.parent = farStarLabels.transform;
            }
        }

    }


    public float[] getPlanetRadii(System system) {

		float[] radii = new float[system.planets.Count];
	
		int i = 0;
		foreach(Planet planet in system.planets) {

			radii[i] = planet.nRadius;
			i = i + 1;
		}

		return radii;
	}


	public float[] getPlanetDistances(System system) {
	
		float[] distances = new float[system.planets.Count];

		int i = 0;
		foreach (Planet planet in system.planets) {

			distances [i] = planet.nDistance;
			i = i + 1;
		}

		return distances;
	}

	public List<System> getSolarSystems() {

        var time = Time.realtimeSinceStartup;
	
		DirectoryInfo d = new DirectoryInfo(Application.dataPath + "/PlanetData/");
		FileInfo[] Files = d.GetFiles("*.xml"); //Getting XML files
		List<String> systemFiles = new List<String>();
		foreach(FileInfo file in Files )
		{
			systemFiles.Add (file.Name); // Getting the list of all XML files present in the folder
		}

		Debug.Log ("No of files: " + systemFiles.Count);

		List<System> solarSystems = new List<System> ();

		foreach (String file in systemFiles) {

			XmlDocument newXml = new XmlDocument ();
			newXml.Load(Application.dataPath + "/PlanetData/" + file); 
			XmlNode root = newXml.DocumentElement;

			System system = new System ();
			system.planets = new List<Planet> ();

			if(!shouldParse (file)) {
				continue;
			}


			foreach(XmlNode node in newXml.DocumentElement.ChildNodes) {

				if (node.Name == "name") { system.name = node.InnerText; }

				if (node.Name == "rightascension") { system.rightascension = node.InnerText; }

				if (node.Name == "declination") { system.declination = node.InnerText; }

				if (node.Name == "distance") { system.distance = node.InnerText; }

				if (node.Name == "star") {

					Star star = new Star ();

					foreach (XmlNode starNode in node.ChildNodes) {

						if (starNode.Name == "name") { star.name = starNode.InnerText; }

						if (starNode.Name == "mass") { star.mass = starNode.InnerText; }

						if (starNode.Name == "radius") { star.radius = starNode.InnerText; }

						if (starNode.Name == "magV") { star.magV = starNode.InnerText; }

						if (starNode.Name == "magB") { star.magB = starNode.InnerText; }

						if (starNode.Name == "magJ") { star.magJ = starNode.InnerText; }

						if (starNode.Name == "magH") { star.magH = starNode.InnerText; }

						if (starNode.Name == "magK") { star.magK = starNode.InnerText; }

						if (starNode.Name == "temperature") { star.temperature = starNode.InnerText; }

						if (starNode.Name == "metallicity") { star.metallicity = starNode.InnerText; }

						if (starNode.Name == "spectraltype") { star.spectraltype = starNode.InnerText; }

						if (starNode.Name == "planet") {

							Planet newPlanet = new Planet ();

							foreach (XmlNode planetNode in starNode.ChildNodes) {

								if (planetNode.Name == "name") { newPlanet.name = planetNode.InnerText; }

								if (planetNode.Name == "mass") { newPlanet.mass = planetNode.InnerText; }

								if (planetNode.Name == "period") { newPlanet.period = planetNode.InnerText; }

								if (planetNode.Name == "radius") {

									if (planetNode.Attributes != null && planetNode.Attributes ["lowerlimit"] != null) {

										newPlanet.radius = planetNode.Attributes ["lowerlimit"].Value;

									} else if (planetNode.Attributes != null && planetNode.Attributes ["upperlimit"] != null) {
									
										newPlanet.radius = planetNode.Attributes ["upperlimit"].Value;
									}
									else {
									
										newPlanet.radius = planetNode.InnerText;
									
									}
									
								}

								if (planetNode.Name == "discoverymethod") { newPlanet.discoverymethod = planetNode.InnerText;}

								if (planetNode.Name == "semimajoraxis") { newPlanet.semimajoraxis = planetNode.InnerText; }

								if (planetNode.Name == "eccentricity") { newPlanet.eccentricity = planetNode.InnerText; }

								if (planetNode.Name == "description") { newPlanet.description = planetNode.InnerText; }

								if (planetNode.Name == "discoverymethod") { newPlanet.discoverymethod = planetNode.InnerText; }

								if (planetNode.Name == "discoveryyear") { newPlanet.discoveryyear = planetNode.InnerText; }

							}
							newPlanet.setupNumericValues (); // This is to convert the obtained string values to numeric values
							system.planets.Add (newPlanet);
						}

					}
					star.setupNumericValues (); // Same as of planet but also to calculate the Luminosity
					system.star = star;
				}

			}

			solarSystems.Add (system);

		}

        //		Debug.Log ("No of valid systems: " + solarSystems.Count);
        //
        //		foreach (System solarSystem in solarSystems) {
        //
        //			Debug.Log ("Solar System : " + solarSystem.name);
        //
        //		}

        float elapsed = Time.realtimeSinceStartup - time;

        Debug.Log("GetSolarSystems took " + elapsed + " seconds");
        Debug.Log("GetSolarSystems system amount: " + solarSystems.Count);
		return solarSystems;
	}



	bool shouldParse(String file) {

		XmlDocument newXml = new XmlDocument ();
		newXml.Load(Application.dataPath + "/PlanetData/" + file); 
		XmlNode root = newXml.DocumentElement;

		XmlNodeList numBinary = newXml.GetElementsByTagName ("binary");
		XmlNodeList numPlanets = newXml.GetElementsByTagName ("planet");
		XmlNodeList numStar = newXml.GetElementsByTagName ("star");
		XmlNodeList temp = newXml.GetElementsByTagName ("temperature");
		XmlNodeList numRadius = newXml.GetElementsByTagName ("radius");
		XmlNodeList numSemimajoraxis = newXml.GetElementsByTagName ("semimajoraxis");
		XmlNodeList numPeriod = newXml.GetElementsByTagName ("period");

		if (numBinary.Count > 0) {
//			Debug.Log (file + " Binary star system");
			return false;
		}

		if (numRadius.Count != (numPlanets.Count + numStar.Count)) {
//			Debug.Log ("numRadius : " + numRadius.Count + " planets: " + numPlanets.Count + " Star :" + numStar.Count);
//			Debug.Log (file + " Radius of the star or planet missing");
			return false;
		}

		if (!(temp.Count >= numStar.Count)) {
		
//			Debug.Log (file + " Temperature of star missing");
			return false;
		}

		if (numSemimajoraxis.Count != numPlanets.Count) {
		
//			Debug.Log (file + " Planets don't have distance info");
			return false;
		}

		if (numPeriod.Count != numPlanets.Count) {
		
//			Debug.Log (file + " Planets don't have Orbital period info");
			return false;
		}

		if (numStar.Count == 1) {
		
			if (!starHasTemperatureInfo (numStar[0])) {
			
				return false;
			}
		
		}

//		Debug.Log (file + " Returning true");
		return true;

	}

	public bool starHasTemperatureInfo(XmlNode starNode) {
	
		bool hastemperature = false;

		foreach (XmlNode node in starNode.ChildNodes) {

			if (node.Name == "temperature") {
				Debug.Log ("Temperature FOUND");
				hastemperature = true;
			}
		
		}

		return hastemperature;
	}

    [Serializable]
	public class Planet {

		public string name { get; set; }
		public string mass { get; set; }
		public string radius { get; set; }
		public string period { get; set; }
		public string semimajoraxis { get; set; }
		public string eccentricity { get; set; }
		public string description { get; set; }
		public string discoverymethod { get; set; }
		public string discoveryyear { get; set; }

		public float nDistance { get; set; }
		public float nRadius { get; set; }
		public float nOrbit { get; set; }

		public void setupNumericValues() {

			nDistance = float.Parse (semimajoraxis);
			Debug.Log ("Name : " + name + "Radius: " + this.radius);
			nRadius = float.Parse(this.radius);
			nOrbit = float.Parse (period);
		}

	}

    [Serializable]
    public class Star {

		public string name { get; set; }
		public string mass { get; set; }
		public string radius { get; set; }
		public string magV { get; set; }
		public string magB { get; set; }
		public string magJ { get; set; }
		public string magH { get; set; }
		public string magK { get; set; }
		public string temperature { get; set; }
		public string metallicity { get; set; }
		public string spectraltype { get; set; }

		public float nRadius { get; set; }
		public double nLuminosity { get; set; }


		public double getLuminosity (float radius, float temperature) {
            double lum = ((4.0F * Mathf.PI * Math.Pow(radius, 2) * 5.670373e-8 * Mathf.Pow(temperature, 4)) / (3.827e+26));
            return lum;
		}

		public void setupNumericValues() {

			Debug.Log ("LUMINOSITY CALC: STAR: "+ name +  " Radius : " + radius + " Temperature: " + temperature);
			nLuminosity = getLuminosity (float.Parse (radius) * 695700000 * .8f, float.Parse (temperature));
			nRadius = float.Parse (radius);
		}

	}

    [Serializable]
    public class System {

		public string name { get; set; }
		public string rightascension { get; set; }
		public string declination { get; set; }
		public string distance { get; set; }

		public Star star { get; set; }
		public List<Planet> planets { get; set; }
	
	}

    void LoadStoredSystems() {
        string filepath = Application.dataPath + "//planetData.dat";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(filepath, FileMode.Open);
        systemData = (List<System>)formatter.Deserialize(saveFile);
        saveFile.Close();

        // This is some ugly code, but it's not bad performance-wise.

        saveFile = File.Open(filepath, FileMode.Open);
        favSystems = (List<System>)formatter.Deserialize(saveFile);
        saveFile.Close();

        saveFile = File.Open(filepath, FileMode.Open);
        brightestSuns = (List<System>)formatter.Deserialize(saveFile);
        saveFile.Close();

        saveFile = File.Open(filepath, FileMode.Open);
        recentSystems = (List<System>)formatter.Deserialize(saveFile);
        saveFile.Close();

        saveFile = File.Open(filepath, FileMode.Open);
        largestSuns = (List<System>)formatter.Deserialize(saveFile);
        saveFile.Close();

        saveFile = File.Open(filepath, FileMode.Open);
        mostPlanets = (List<System>)formatter.Deserialize(saveFile);
        saveFile.Close();


    }

    void SaveSystemsToFile() {
        string filepath = Application.dataPath + "//planetData.dat";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(filepath);
        formatter.Serialize(saveFile, getSolarSystems());
        saveFile.Close();
    }

    void CreateFilteredLists(){

        LargestSunsSystemList();
        MostPlanets();
        RecentlyDiscovered();
        BrightestSuns();
        FavoriteSystems();

    }

    void LargestSunsSystemList() {
        largestSuns.RemoveAt(3112);
        largestSuns.Sort((x, y) => x.star.nRadius.CompareTo(y.star.nRadius));
        largestSuns.Reverse();

        largestSuns.RemoveRange(150, largestSuns.Count- 151);
    }

    void MostPlanets() {
        mostPlanets.RemoveAt(3112);
        mostPlanets.Sort((x, y) => x.planets.Count.CompareTo(y.planets.Count));
        mostPlanets.Reverse();

        mostPlanets.RemoveRange(150, mostPlanets.Count - 151);
    }

    void RecentlyDiscovered() {
        recentSystems.RemoveAt(3112);

        for (int i = 0; i < recentSystems.Count; i++) {
            if (recentSystems[i].planets[0].discoveryyear == null || recentSystems[i].planets[0].discoveryyear == "") {
                recentSystems[i].planets[0].discoveryyear = "0";
            }
        }

        recentSystems.Sort((x, y) => x.planets[0].discoveryyear.CompareTo(y.planets[0].discoveryyear));
        recentSystems.Reverse();

        recentSystems.RemoveRange(150, recentSystems.Count - 151);
    }

    void BrightestSuns() {
        brightestSuns.RemoveAt(3112);
        brightestSuns.Sort((x, y) => x.star.nLuminosity.CompareTo(y.star.nLuminosity));
        brightestSuns.Reverse();

        brightestSuns.RemoveRange(150, brightestSuns.Count - 151);   
    }

    void FavoriteSystems() {
        favSystems = new List<System>();
        // ADD 4 SYSTEMS TO THE FAVSYSTEMS LIST, TO SEE THEM IN THEIR CATEGORY IN SYSTEM MENU.

    }

    void CreateSol() {
        sb.dealWithNewSystem(systemsDict["Sun"], allCenter);
        sb.Create2dView(systemsDict["Sun"], allSide);
    }





}
