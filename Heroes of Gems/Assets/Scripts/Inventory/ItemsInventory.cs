using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemsInventory : BaseInventory {
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<GameObject> itemsGO = new List<GameObject>();

    private void Start() {
        FillInventory();
    }

    protected override void FillInventory() {
        assetGuids = AssetDatabase.FindAssets("t:Item", new string[] { folderPath });
        for (int i = 0; i < assetGuids.Length; i++) {
            GameObject itemGO = Instantiate(emptyItemPrefab);
            itemGO.name = "ItemSlot" + i;
            itemGO.transform.SetParent(transform, false);
            //itemGO.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
            //itemGO.GetComponentInChildren<DragItem>().SetCanvas(inventoryCanvas.GetComponent<Canvas>());
            //itemGO.GetComponentInChildren<DragItem>().SetParent(container);

            GameObject grayScaleGO = GameObject.Find("GrayScale");
            grayScaleGO.name += i;

            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);

            Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
            itemGO.AddComponent<ItemSlot>();
            itemGO.GetComponent<ItemSlot>().item = item;
            ItemDisplay itemUI = itemGO.GetComponent<ItemDisplay>();

            itemUI.grayScale = grayScaleGO;

            if (i == 1 || i == 5) {
                items.Add(item);
                items.Add(item);
                items.Add(item);
            }

            //int itemCount = items.Count(it => it.Name == item.Name);

            int itemCount = items.Count(it => it.itemName == item.itemName);

            if (itemCount > 0) {
                if (itemGO.transform.GetChild(0).gameObject.GetComponent<DragItem>() == null) {
                    itemGO.transform.GetChild(0).gameObject.AddComponent<DragItem>();
                    itemGO.GetComponentInChildren<DragItem>().originalGO = itemGO.transform.GetChild(0).gameObject;
                    itemGO.GetComponentInChildren<DragItem>().SetCanvas(inventoryCanvas.GetComponent<Canvas>());
                    itemGO.GetComponentInChildren<DragItem>().SetParent(container);
                }
            }
            else {
                if (itemGO.GetComponentInChildren<DragItem>() != null) {
                    Destroy(itemGO.GetComponentInChildren<DragItem>());
                }
            }

            itemUI.ChangeGrayScale(itemCount > 0 ? false : true);

            itemUI.ChangeItemCounter(itemCount);
            itemUI.ChangeItemImage(item.itemIcon);

            itemsGO.Add(itemGO);
        }
    }

    public void AddItem(Item item) {
        items.Add(item);
        RefreshUI();
    }

    public void RemoveItem(Item item) {
        items.Remove(item);
        RefreshUI();
    }

    protected override void RefreshUI() {
        foreach (GameObject itemGO in itemsGO) {
            ItemDisplay itemUI = itemGO.GetComponent<ItemDisplay>();

            int itemCount = items.Count(it => it.itemName == itemGO.GetComponent<ItemSlot>().item.itemName);

            itemUI.ChangeGrayScale(itemCount > 0 ? false : true);

            if (itemCount > 0) {
                if (itemGO.transform.GetChild(0).gameObject.GetComponent<DragItem>() == null) {
                    itemGO.transform.GetChild(0).gameObject.AddComponent<DragItem>();
                    itemGO.GetComponentInChildren<DragItem>().originalGO = itemGO.transform.GetChild(0).gameObject;
                    itemGO.GetComponentInChildren<DragItem>().SetCanvas(inventoryCanvas.GetComponent<Canvas>());
                    itemGO.GetComponentInChildren<DragItem>().SetParent(container);
                }
            }
            else {
                if (itemGO.GetComponentInChildren<DragItem>() != null) {
                    Destroy(itemGO.GetComponentInChildren<DragItem>());
                }
            }

            itemUI.ChangeItemCounter(itemCount);
        }
    }

    public int ItemCount(Item item) {
        return items.Count(it => it.itemName == item.itemName);
    }
}