using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityHandler : MonoBehaviour, IDataPersistence {
    private static List<GameObject> citiesGO = new List<GameObject>();
    private static List<CityObjectData> citiesObjGO = new List<CityObjectData>();

    private void Awake() {
        citiesGO = GameObject.FindGameObjectsWithTag("City").ToList();
    }

    public void Start() {
        foreach (GameObject cityGO in citiesGO) {
            cityGO.GetComponent<ShowCity>().LoadCityCont();
        }
    }

    public static List<CityObjectData> GetCities() {
        return citiesObjGO;
    }

    public static void SetCities(List<CityObjectData> cityObjects) {
        citiesObjGO = cityObjects;
    }

    public void LoadData(GameData gameData) {
        citiesObjGO.Clear();
        citiesObjGO = gameData.cities;
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