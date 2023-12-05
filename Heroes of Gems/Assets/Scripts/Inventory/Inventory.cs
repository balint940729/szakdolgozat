using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public GameObject emptyItemPrefab;
    [SerializeField] private List<Item> items;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private ItemSlot[] itemSlots;
    public int numberOfItems;
    private string folderPath = "Assets/Sprites/Items";
    private string[] assetGuids;

    private void Start() {
        //if (itemsParent != null) {
        //    itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        //}
        for (int i = 0; i < numberOfItems; i++) {
            GameObject itemGO = Instantiate(emptyItemPrefab);
            itemGO.name = "ItemSlot" + i;
            itemGO.transform.SetParent(itemsParent.transform, false);
            assetGuids = AssetDatabase.FindAssets("t:Item", new string[] { folderPath });

            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[1]);

            Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
            GameObject icon = GameObject.Find("Icon");
            icon.name = "Icon" + i;
            Image img = icon.GetComponent<Image>();
            img.sprite = item.Icon;
        }
        RefreshUI();
    }

    private void RefreshUI() {
        for (int i = 0; i < items.Count && i < itemSlots.Length; i++) {
            itemSlots[i].Item = items[i];
        }

        for (int i = 0; i < itemSlots.Length; i++) {
            itemSlots[i].Item = null;
        }
    }

    public bool AddItem(Item item) {
        if (IsFull()) {
            return false;
        }

        items.Add(item);
        RefreshUI();
        return true;
    }

    public bool RemoveItem(Item item) {
        if (items.Remove(item)) {
            RefreshUI();
            return true;
        }
        return false;
    }

    public bool IsFull() {
        return items.Count >= itemSlots.Length;
    }
}