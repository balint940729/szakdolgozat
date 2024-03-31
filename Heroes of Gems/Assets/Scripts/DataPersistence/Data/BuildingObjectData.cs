using UnityEngine;

[System.Serializable]
public class BuildingObjectData {
    [SerializeField] public Building building;
    [SerializeField] public int level;

    public BuildingObjectData(Building building, int level) {
        this.building = building;
        this.level = level;
    }
}