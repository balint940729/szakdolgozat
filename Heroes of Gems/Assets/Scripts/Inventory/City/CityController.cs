using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {
    public City city;
    public List<BuildingController> cityBuildings;
    private static List<CityController> activeCities = new List<CityController>();

    public static List<CityController> GetActiveCities() {
        return activeCities;
    }

    public static void AddActiveCity(CityController cityC) {
        activeCities.Add(cityC);
    }

    public static void SetActiveCities(CityController cityC) {
        activeCities.Add(cityC);
    }
}