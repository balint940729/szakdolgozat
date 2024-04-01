using System.Collections.Generic;
using UnityEngine;

public class CityContainer : MonoBehaviour {
    [SerializeField] private GameObject buildingPrefab = default;

    public List<BuildingController> InitializeCityCont(City city) {
        List<BuildingController> buildingControllers = new List<BuildingController>();
        foreach (Building building in city.buildings) {
            GameObject buildingGO = Instantiate(buildingPrefab);
            buildingGO.transform.SetParent(transform, false);

            buildingGO.GetComponent<BuildingController>().SetUpBuilding(building);

            buildingControllers.Add(buildingGO.GetComponent<BuildingController>());
        }

        return buildingControllers;
    }

    public List<BuildingController> InitializeCityCont(List<BuildingObjectData> buildingsObjs) {
        List<BuildingController> buildingControllers = new List<BuildingController>();
        foreach (BuildingObjectData buildingObj in buildingsObjs) {
            GameObject buildingGO = Instantiate(buildingPrefab);
            buildingGO.transform.SetParent(transform, false);

            buildingGO.GetComponent<BuildingController>().SetUpBuilding(buildingObj.building, buildingObj.level);

            buildingControllers.Add(buildingGO.GetComponent<BuildingController>());
        }

        return buildingControllers;
    }
}