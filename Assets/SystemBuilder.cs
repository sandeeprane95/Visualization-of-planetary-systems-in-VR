using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SystemBuilder : MonoBehaviour {

    float panelXScale = 1;
    float orbitXScale = 1;

    float orbitWidth = 0.01F;
    float habWidth = 0.03F;

    float revolutionSpeed = 10F;

    float panelHeight = 0.1F;
    float panelWidth = 60.0F;
    float panelDepth = 0.001F;

    static float orbitScaleMult = 1;
    static float revolutionSpeedMult = 1;
    static float planetScaleMult = 1;

    public static void ChangeOptions(SimOptions o)
    {
        orbitScaleMult = o.oScale;
        revolutionSpeedMult = o.revScale;
        planetScaleMult = o.pScale;
    }

    public void Create2dView(Planets.System sys, GameObject sideParent) {

        GameObject SolarSide;
        SolarSide = new GameObject();
        SolarSide.name = "Side View of " + sys.name;
        SolarSide.tag = "SideViewHolder";
        if (sideParent != null)
            SolarSide.transform.parent = sideParent.transform;
        if (sys.star.name != "Sun")
            SolarSide.AddComponent<FlatSystemSlot>();
        else {
            SolarSide.transform.position = new Vector3(30, 70, 50);
        }

        sideDealWithStar(sys.star, SolarSide);
        sideDealWithPlanets(sys.planets, SolarSide);
    }

    public void dealWithNewSystem(Planets.System sys, GameObject allThings) {



        GameObject SolarCenter;
        GameObject AllOrbits;
        GameObject SunStuff;
        GameObject Planets;

        SolarCenter = new GameObject();
        AllOrbits = new GameObject();
        SunStuff = new GameObject();
        Planets = new GameObject();

        SolarCenter.name = "SolarCenter" + " " + sys.name;
        AllOrbits.name = "All Orbits" + " " + sys.name;
        SunStuff.name = "Sun Stuff" + " " + sys.name;
        Planets.name = "Planets" + " " + sys.name;

        SolarCenter.transform.parent = allThings.transform;

        AllOrbits.transform.parent = SolarCenter.transform;
        SunStuff.transform.parent = SolarCenter.transform;
        Planets.transform.parent = SolarCenter.transform;

        if (sys.star.name != "Sun")
            SolarCenter.AddComponent<SystemSlot>();

        dealWithNewStar(sys.star, SunStuff, AllOrbits, sys);
        dealWithNewPlanets(sys.planets, Planets, AllOrbits);

        // This script communicates with SlotManager to position the systems correctly.
        // Our Sun should always be in the scene, so it doesn't need one.
        // The system takes that into account.

    }

    void dealWithNewStar(Planets.Star star, GameObject thisStar, GameObject theseOrbits, Planets.System sysInfo = null) {

        GameObject newSun, upperSun;
        Material sunMaterial;

        GameObject sunRelated;
        GameObject sunSupport;
        GameObject sunText;

        float sunScale = float.Parse(star.radius) * 2f; // 100000F;
        float centerSunSize = 0.25F;
        float luminosity = (float)star.nLuminosity;

        // set the habitable zone based on the star's luminosiy
        float innerHab = luminosity * 9.5F;
        float outerHab = luminosity * 14F;

        newSun = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newSun.AddComponent<rotate>();
        newSun.name = star.name;
        newSun.transform.position = new Vector3(0, 0, 0);
        newSun.transform.localScale = new Vector3(centerSunSize, centerSunSize, centerSunSize);

        sunRelated = thisStar;

        newSun.GetComponent<rotate>().rotateSpeed = -0.25F;

        sunMaterial = new Material(Shader.Find("Unlit/Texture"));
        newSun.GetComponent<MeshRenderer>().material = sunMaterial;
        sunMaterial.mainTexture = Resources.Load("gstar") as Texture;

        newSun.transform.parent = sunRelated.transform;


        // copy the sun and make a bigger version above
        upperSun = Instantiate(newSun);
        upperSun.name = star.name + " upper";
        upperSun.transform.localScale = new Vector3(sunScale, sunScale, sunScale);
        upperSun.transform.position = new Vector3(0, 10, 0);

        upperSun.transform.parent = sunRelated.transform;
        upperSun.AddComponent<SunLaserDetection>();
        upperSun.AddComponent<SystemInfoHandler>();
        upperSun.GetComponent<SystemInfoHandler>().sysInfo = sysInfo;

        // draw the support between them
        sunSupport = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sunSupport.transform.localScale = new Vector3(0.1F, 10.0F, 0.1F);
        sunSupport.transform.position = new Vector3(0, 5, 0);
        sunSupport.name = "Sun Support";

        sunSupport.transform.parent = sunRelated.transform;


        sunText = new GameObject();
        sunText.name = "Star Name";
        sunText.transform.position = new Vector3(0, 5, 0);
        sunText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var sunTextMesh = sunText.AddComponent<TextMesh>();
        sunTextMesh.text = star.name;
        sunTextMesh.fontSize = 200;

        sunText.transform.parent = sunRelated.transform;


        drawOrbit("Habitable Inner Ring", innerHab * orbitXScale * orbitScaleMult, Color.green, habWidth, theseOrbits);
        drawOrbit("Habitable Outer Ring", outerHab * orbitXScale * orbitScaleMult, Color.green, habWidth, theseOrbits);
    }

    void dealWithNewPlanets(List<Planets.Planet> planets, GameObject thesePlanets, GameObject theseOrbits) {
        GameObject newPlanetCenter;
        GameObject newPlanet;

        GameObject sunRelated;

        Material planetMaterial;

        int planetCounter;

        for (planetCounter = 0; planetCounter < planets.Count; planetCounter++) {

            float planetDistance = planets[planetCounter].nDistance * 7f * orbitScaleMult; // 149600000.0F * 10.0F;
            float planetSize = planets[planetCounter].nRadius * 3.0F * planetScaleMult;  // 10000.0F;
            float planetSpeed = -1.0F / planets[planetCounter].nOrbit * revolutionSpeed * revolutionSpeedMult;
            string textureName = GetPlanetTextureName(planets[planetCounter].name);
            string planetName = planets[planetCounter].name;

            newPlanetCenter = new GameObject(planetName + "Center");
            newPlanetCenter.AddComponent<rotate>();
            // Add a tag to be able to quickly find all planet centers.
            newPlanetCenter.tag = "PlanetCenter";

            newPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newPlanet.name = planetName;
            newPlanet.transform.position = new Vector3(0, 0, planetDistance * orbitXScale);
            newPlanet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
            newPlanet.transform.parent = newPlanetCenter.transform;
            // Add tag to be able to quickly find all planets.
            newPlanet.tag = "Planet";


            newPlanetCenter.GetComponent<rotate>().rotateSpeed = planetSpeed;

            planetMaterial = new Material(Shader.Find("Standard"));
            newPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
            planetMaterial.mainTexture = Resources.Load(textureName) as Texture;

            drawOrbit(planetName + " orbit", planetDistance * orbitXScale, Color.white, orbitWidth, theseOrbits);

            sunRelated = thesePlanets;
            newPlanetCenter.transform.parent = sunRelated.transform;
        }
    }



    /*
     * CODE FOR BUILDING 2D SIDE VIEWS
     */

    void sideDealWithPlanets(List<Planets.Planet> planets, GameObject thisSide) {
        GameObject newPlanet;

        GameObject sunRelated;

        Material planetMaterial;

        int planetCounter;
        int planetsOutOfView = 0;

        for (planetCounter = 0; planetCounter < planets.Count; planetCounter++) {

            float planetDistance = planets[planetCounter].nDistance * 7f * orbitScaleMult; // 149600000.0F * 10.0F;
            float planetSize = planets[planetCounter].nRadius * 3.0F * planetScaleMult; // 10000.0F;
            string textureName = GetPlanetTextureName(planets[planetCounter].name);
            string planetName = planets[planetCounter].name;

            // limit the planets to the width of the side view
            if ((panelXScale * planetDistance) < panelWidth) {
                sunRelated = thisSide;
                newPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newPlanet.transform.parent = sunRelated.transform;
                newPlanet.name = planetName;
                newPlanet.transform.position = new Vector3(-0.5F * panelWidth + planetDistance * panelXScale, 0, 0) + thisSide.transform.position;
                newPlanet.transform.localScale = new Vector3(planetSize, planetSize, 5.0F * panelDepth);

                planetMaterial = new Material(Shader.Find("Standard"));
                newPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
                planetMaterial.mainTexture = Resources.Load(textureName) as Texture;

                
                
            } else {
                planetsOutOfView++;
            }

        }


        if (planetsOutOfView > 0) {
            var morePlanets = new GameObject();
            morePlanets.transform.parent = thisSide.transform;
            morePlanets.name = "Invisible planets indicator";
            morePlanets.transform.position = new Vector3(panelWidth / 2 + .5f, .5f, 0) + thisSide.transform.position;
            morePlanets.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
            var morePlanetsTxt = morePlanets.AddComponent<TextMesh>();
            morePlanetsTxt.text = planetsOutOfView + " more";
            morePlanetsTxt.fontSize = 75;
        }
    }

    //------------------------------------------------------------------------------------//

    void sideDealWithStar(Planets.Star star, GameObject thisSide) {
        GameObject newSidePanel;
        GameObject newSideSun;
        GameObject sideSunText;

        GameObject habZone;

        Material sideSunMaterial, habMaterial;

        int luminosity = (int)star.nLuminosity;
        // Creates the axis
        newSidePanel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newSidePanel.transform.parent = thisSide.transform;
        newSidePanel.name = "Side " + star.name + " Panel";
        newSidePanel.transform.position = new Vector3(0, 0, 0) + thisSide.transform.position;
        newSidePanel.transform.localScale = new Vector3(panelWidth, panelHeight, panelDepth);
        

        // Creates sun block
        newSideSun = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newSideSun.transform.parent = thisSide.transform;
        newSideSun.name = "Side " + star.name + " Star";
        newSideSun.transform.position = new Vector3(-0.5F * panelWidth - 0.5F, 0, 0) + thisSide.transform.position;
        newSideSun.transform.localScale = new Vector3(1.0F, panelHeight * 40.0F, 2.0F * panelDepth);
        

        sideSunMaterial = new Material(Shader.Find("Unlit/Texture"));
        newSideSun.GetComponent<MeshRenderer>().material = sideSunMaterial;
        sideSunMaterial.mainTexture = Resources.Load("sun") as Texture;

        // Creates caption
        sideSunText = new GameObject();
        sideSunText.transform.parent = thisSide.transform;
        sideSunText.name = "Side Star Name";
        sideSunText.transform.position = new Vector3(-0.47F * panelWidth, 22.0F * panelHeight, 0) + thisSide.transform.position;
        sideSunText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var sunTextMesh = sideSunText.AddComponent<TextMesh>();
        sunTextMesh.text = star.name;
        sunTextMesh.fontSize = 150;


        // Create collider to catch clicks.
        BoxCollider col = newSidePanel.GetComponent<BoxCollider>();
        col.size = new Vector3(col.size.x, col.size.y * 40, col.size.z * 1.2f);
        col.isTrigger = true;
        newSidePanel.AddComponent<FlatViewLaserDetection>();

        if (star.name == "Sun") return;

        float innerHab = luminosity * 9.5F;
        float outerHab = luminosity * 14F;


        // need to take panelXScale into account for the hab zone

        habZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
        habZone.transform.parent = thisSide.transform;
        habZone.name = "Hab";
        habZone.transform.position = new Vector3((-0.5F * panelWidth) + ((innerHab + outerHab) * 0.5F * panelXScale), 0, 0) + thisSide.transform.position;
        habZone.transform.localScale = new Vector3((outerHab - innerHab) * panelXScale, 40.0F * panelHeight, 2.0F * panelDepth) + thisSide.transform.position;
        

        habMaterial = new Material(Shader.Find("Standard"));
        habZone.GetComponent<MeshRenderer>().material = habMaterial;
        habMaterial.mainTexture = Resources.Load("habitable") as Texture;




    }

    void drawOrbit(string orbitName, float orbitRadius, Color orbitColor, float myWidth, GameObject myOrbits) {

        GameObject newOrbit;
        GameObject orbits;


        newOrbit = new GameObject(orbitName);
        newOrbit.AddComponent<Circle>();
        newOrbit.AddComponent<LineRenderer>();

        newOrbit.GetComponent<Circle>().xradius = orbitRadius;
        newOrbit.GetComponent<Circle>().yradius = orbitRadius;

        var line = newOrbit.GetComponent<LineRenderer>();
        line.startWidth = myWidth;
        line.endWidth = myWidth;
        line.useWorldSpace = false;

        newOrbit.GetComponent<LineRenderer>().material.color = orbitColor;

        orbits = myOrbits;
        newOrbit.transform.parent = orbits.transform;


    }


    string GetPlanetTextureName(string planetName)
    {
        if (planetName == "Sun b") return "mercury";
        if (planetName == "Sun c") return "venus";
        if (planetName == "Sun d") return "earthmap";
        if (planetName == "Sun e") return "mars";
        if (planetName == "Sun f") return "jupiter";
        if (planetName == "Sun g") return "saturn";
        if (planetName == "Sun h") return "uranus";
        if (planetName == "Sun i") return "neptune";
        //if (planetName == "Sun j") return "pluto"; don't have a pluto texture.

        string[] allTextures = new string[] { "mercury", "venus", "mars", "jupiter", "saturn", "uranus", "neptune",
        "other0", "other1", "other2", "other3", "other4", "other5"};

        int luckyNumber = Random.Range(0, allTextures.Length - 1);

        return allTextures[luckyNumber];

    }
}
