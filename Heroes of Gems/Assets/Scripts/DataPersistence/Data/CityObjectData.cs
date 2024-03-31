using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityObjectData {
    [SerializeField] public string cityName;
    [SerializeField] public List<BuildingObjectData> buildings;

    public CityObjectData(string cityName, List<BuildingObjectData> buildings) {
        this.cityName = cityName;
        this.buildings = buildings;
    }
}