using System.Collections.Generic;
using UnityEngine;

public class CityContainer : MonoBehaviour {
    [SerializeField] private GameObject buildingPrefab = default;

    public void InitializeCityCont(City city, List<BuildingController> buildingControllers) {
        foreach (Building building in city.buildings) {
            GameObject buildingGO = Instantiate(buildingPrefab);
            buildingGO.transform.SetParent(transform, false);

            buildingGO.GetComponent<BuildingController>().SetUpBuilding(building);

            buildingControllers.Add(buildingGO.GetComponent<BuildingController>());
        }
    }
}