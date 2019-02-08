using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    [SerializeField]
    GameObject mainMenu, optionsMenu, systemsMenu, viveCam, initialCamPos, systemList, listEntry, categories;
    [SerializeField]
    ScrollRect scrl;

	// Use this for initialization
	void Start () {
        StartCoroutine(SetupPlanetListDelayed());
    }

    IEnumerator SetupPlanetListDelayed() {
        yield return new WaitForSeconds(.2f);
        SetupPlanetList(0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleActive(GameObject thing) {
        thing.SetActive(!thing.activeInHierarchy);
    }

    public void ToggleGreyedOut(Button button) {
        button.interactable = !button.IsInteractable();
    }

    public void ShowOptionsPanel() {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void BackToMainMenu() {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

	public void ResetCamPosition() {
		viveCam.transform.position = initialCamPos.transform.position;
		viveCam.transform.rotation = initialCamPos.transform.rotation;

		}

    public void SetupPlanetList(int mode = 0) {
        // Clear list.
        foreach (Transform item in systemList.transform) {
            Destroy(item.gameObject);
        }

        if (mode == 0) {
            foreach (var item in Planets.instance.filteredList) {
                var newEntry = Instantiate(listEntry, systemList.transform);
                newEntry.GetComponent<PlanetListItem>().sys = item;
                newEntry.SetActive(true);
            }
            scrl.verticalNormalizedPosition = 0;
        }

        if (mode == 1) {
            foreach (var item in Planets.instance.favSystems) {
                var newEntry = Instantiate(listEntry, systemList.transform);
                newEntry.GetComponent<PlanetListItem>().sys = item;
                newEntry.SetActive(true);
            }
            scrl.verticalNormalizedPosition = 0;
        }

        if (mode == 2) {
            foreach (var item in Planets.instance.recentSystems) {
                var newEntry = Instantiate(listEntry, systemList.transform);
                newEntry.GetComponent<PlanetListItem>().sys = item;
                newEntry.SetActive(true);
            }
            scrl.verticalNormalizedPosition = 0;
        }

        if (mode == 3) {
            foreach (var item in Planets.instance.brightestSuns) {
                var newEntry = Instantiate(listEntry, systemList.transform);
                newEntry.GetComponent<PlanetListItem>().sys = item;
                newEntry.SetActive(true);
            }
            scrl.verticalNormalizedPosition = 0;
        }

        if (mode == 4) {
            foreach (var item in Planets.instance.mostPlanets) {
                var newEntry = Instantiate(listEntry, systemList.transform);
                newEntry.GetComponent<PlanetListItem>().sys = item;
                newEntry.SetActive(true);
            }
            scrl.verticalNormalizedPosition = 0;
        }

        if (mode == 5) {
            foreach (var item in Planets.instance.largestSuns) {
                var newEntry = Instantiate(listEntry, systemList.transform);
                newEntry.GetComponent<PlanetListItem>().sys = item;
                newEntry.SetActive(true);
            }
            scrl.verticalNormalizedPosition = 0;
        }
        HideCategories();
    }

    public void DisplayCategories() {
        systemList.SetActive(false);
        categories.SetActive(true);
    }

    public void HideCategories() {
        categories.SetActive(false);
        systemList.SetActive(true);
    }

    public void Scroll(float amount)
    {
        scrl.verticalNormalizedPosition += amount;
    }

    public void QuitSim() {

        Application.Quit();
    }
}
