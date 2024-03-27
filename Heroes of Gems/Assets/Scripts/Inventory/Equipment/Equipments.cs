using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipments : MonoBehaviour, IDataPersistence {
    [SerializeField] private static List<Item> equipments = new List<Item>();

    public static List<Item> GetEquipments() {
        return equipments;
    }

    public static void SetEquipments(List<Item> items) {
        equipments = items;
    }

    public static void AddEquipment(Item item) {
        equipments.Add(item);
    }

    public static void RemoveEquipment(Item item) {
        equipments.Remove(item);
    }

    public void LoadData(GameData gameData) {
        SetEquipments(gameData.equipments);
    }

    public void SaveData(ref GameData gameData) {
        gameData.equipments = GetEquipments();
    }
}