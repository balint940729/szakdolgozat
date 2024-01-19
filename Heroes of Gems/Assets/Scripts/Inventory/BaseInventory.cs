using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventory : MonoBehaviour {
    public GameObject emptyItemPrefab;

    public string folderPath;

    protected string[] assetGuids;

    protected List<Item> items;

    public GameObject inventoryCanvas = default;
    public RectTransform container = default;

    //public Transform itemsParent;
    public ItemSlot[] itemSlots;

    public List<GameObject> otherPrefabs;

    //public BaseInventory(GameObject emptyItemPrefab, string folderPath) {
    //    this.emptyItemPrefab = emptyItemPrefab;
    //    this.folderPath = folderPath;
    //}

    protected abstract void FillInventory();

    public void SetEmptyItemPrefab(GameObject emptyItemPrefab) {
        this.emptyItemPrefab = emptyItemPrefab;
    }

    protected virtual void RefreshUI() {
        for (int i = 0; i < items.Count && i < itemSlots.Length; i++) {
            itemSlots[i].Item = items[i];
        }

        for (int i = 0; i < itemSlots.Length; i++) {
            itemSlots[i].Item = null;
        }
    }

    public virtual bool AddItem(Item item) {
        if (IsFull()) {
            return false;
        }

        items.Add(item);
        RefreshUI();
        return true;
    }

    public virtual void AddUnit(Unit unit) {
        // Implement
    }

    public virtual bool RemoveItem(Item item) {
        if (items.Remove(item)) {
            RefreshUI();
            return true;
        }
        return false;
    }

    public virtual bool IsFull() {
        return items.Count >= itemSlots.Length;
    }
}