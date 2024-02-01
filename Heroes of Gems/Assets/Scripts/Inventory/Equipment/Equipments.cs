using System.Collections.Generic;
using UnityEngine;

public static class Equipments {
    [SerializeField] private static List<Item> equipments;

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
}