using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityHandler : MonoBehaviour, IDataPersistence {
    private static List<GameObject> citiesGO = new List<GameObject>();

    private void Awake() {
        citiesGO = GameObject.FindGameObjectsWithTag("City").ToList();
    }

    public List<GameObject> GetCities() {
        return citiesGO;
    }

    public void LoadData(GameData gameData) {
        foreach (CityObjectData cityObject in gameData.cities) {
            GameObject cityGO = citiesGO.First(c => c.GetComponent<CityController>().city.cityName == cityObject.cityName);
            CityController cityController = cityGO.GetComponent<CityController>();
            CityController.AddActiveCity(cityController);

            foreach (BuildingObjectData buildingObject in cityObject.buildings) {
                //cityController
            }
        }
    }

    public void SaveData(ref GameData gameData) {
        List<CityObjectData> cities = new List<CityObjectData>();
        foreach (CityController cityController in CityController.GetActiveCities()) {
            List<BuildingObjectData> buildings = new List<BuildingObjectData>();

            foreach (BuildingController buildingController in cityController.cityBuildings) {
                BuildingObjectData buildingObject = new BuildingObjectData(buildingController.GetBuilding(), buildingController.GetLevel());
                buildings.Add(buildingObject);
            }
            CityObjectData cityObjectData = new CityObjectData(cityController.city.cityName, buildings);
            cities.Add(cityObjectData);
        }
        gameData.cities = cities;
    }
}