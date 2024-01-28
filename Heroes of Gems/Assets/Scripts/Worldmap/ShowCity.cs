using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCity : MonoBehaviour {
    [SerializeField] private GameObject cityUI = default;
    [SerializeField] private GameObject cityContainer = default;
    [SerializeField] private GameObject closeButton = default;

    private City city;
    private static List<GameObject> cityContainers = new List<GameObject>();

    private void Start() {
        city = GetComponent<CityController>().city;
        List<BuildingController> buildingsCtrl = GetComponent<CityController>().cityBuildings;
        if (buildingsCtrl.Count == 0) {
            GameObject cityCont = Instantiate(cityContainer);
            cityCont.name = city.name + "CityContainer";
            cityCont.transform.SetParent(cityUI.transform, false);
            cityCont.GetComponent<CityContainer>().InitializeCityCont(city, buildingsCtrl);
            cityContainers.Add(cityCont);
        }
        closeButton.GetComponentInChildren<Button>().onClick.AddListener(ShowCityCont);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            ShowCityCont();
        }
    }

    private void ShowCityCont() {
        if (PauseStateHandler.IsGamePaused()) {
            Resume();
        }
        else {
            cityUI.GetComponentInParent<CityDisplay>().ChangeTitle(city.cityName);
            List<GameObject> inactiveConts = cityContainers.FindAll(c => !c.name.Contains(city.cityName));

            foreach (GameObject inact in inactiveConts) {
                inact.SetActive(false);
            }

            //else {
            //
            //    cityCont.SetActive(true);
            //}

            Pause();
        }
    }

    private void Resume() {
        cityUI.SetActive(false);
        closeButton.SetActive(false);

        foreach (GameObject cityCont in cityContainers) {
            cityCont.SetActive(false);
        }

        Time.timeScale = 1f;
        PauseStateHandler.SetGamePause(false);
    }

    private void Pause() {
        cityUI.SetActive(true);
        closeButton.SetActive(true);
        Time.timeScale = 0f;
        PauseStateHandler.SetGamePause(true);
    }
}