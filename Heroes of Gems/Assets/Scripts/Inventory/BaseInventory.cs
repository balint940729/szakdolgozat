using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventory : MonoBehaviour {
    public GameObject emptyItemPrefab;

    public string folderPath;

    protected string[] assetGuids;

    public GameObject inventoryCanvas = default;
    public RectTransform container = default;
    public List<GameObject> otherPrefabs;

    protected abstract void FillInventory();

    public void SetEmptyItemPrefab(GameObject emptyItemPrefab) {
        this.emptyItemPrefab = emptyItemPrefab;
    }

    protected virtual void RefreshUI() {
    }
}