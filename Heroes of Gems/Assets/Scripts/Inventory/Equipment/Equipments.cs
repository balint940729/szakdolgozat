using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipments : MonoBehaviour, IDataPersistence {
    [SerializeField] private static List<EquipmentsObjectData> equipments = new List<EquipmentsObjectData>();

    public static List<EquipmentsObjectData> GetEquipments() {
        return equipments;
    }

    public static void SetEquipments(List<EquipmentsObjectData> items) {
        equipments = items;
    }

    public static void AddEquipment(Item item, ItemType equipmentType) {
        EquipmentsObjectData equipment = new EquipmentsObjectData(item, equipmentType);
        equipments.Add(equipment);
    }

    public static void RemoveEquipment(Item item, ItemType equipmentType) {
        EquipmentsObjectData equipment = new EquipmentsObjectData(item, equipmentType);
        equipments.Remove(equipment);
    }

    public void LoadData(GameData gameData) {
        SetEquipments(gameData.equipments);
    }

    public void SaveData(ref GameData gameData) {
        gameData.equipments = GetEquipments();
    }
}