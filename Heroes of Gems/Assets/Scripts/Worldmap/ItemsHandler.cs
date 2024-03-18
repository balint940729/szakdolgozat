using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsHandler : MonoBehaviour, IDataPersistence {
    private static List<Item> items = new List<Item>();

    public static void AddItem(Item item) {
        items.Add(item);
    }

    public static void RemoveItem(Item item) {
        items.Remove(item);
    }

    public static List<Item> GetItems() {
        return items;
    }

    public static void SetItems(List<Item> itemsList) {
        items = itemsList;
    }

    public void LoadData(GameData gameData) {
        SetItems(gameData.items);
    }

    public void SaveData(ref GameData gameData) {
        gameData.items = GetItems();
    }
}