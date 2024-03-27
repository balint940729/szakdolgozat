using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitsHandler : MonoBehaviour, IDataPersistence {

    [SerializeField]
    private static List<Unit> units = new List<Unit>();

    public static void AddUnit(Unit unit) {
        units.Add(unit);
    }

    public static void RemoveUnit(Unit unit) {
        units.Remove(unit);
    }

    public static List<Unit> GetUnits() {
        return units;
    }

    public static void SetUnits(List<Unit> unitsList) {
        units = unitsList;
    }

    public void LoadData(GameData gameData) {
        SetUnits(gameData.units);
    }

    public void SaveData(ref GameData gameData) {
        gameData.units = GetUnits();
    }
}