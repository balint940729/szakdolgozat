using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemsInventory : MonoBehaviour {
    public GameObject emptyItemPrefab;

    [SerializeField] private List<Item> items;

    public Transform itemsParent;
    public ItemSlot[] itemSlots;

    //public int numberOfItems;
    public string folderPath = "Assets/Sprites/Items/";

    private string[] assetGuids;

    // Start is called before the first frame update
    private void Start() {
        //if (itemsParent != null) {
        //    itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        //}

        assetGuids = AssetDatabase.FindAssets("t:Item", new string[] { folderPath });
        for (int i = 0; i < assetGuids.Length; i++) {
            GameObject itemGO = Instantiate(emptyItemPrefab);
            itemGO.name = "ItemSlot" + i;
            //itemGO.transform.SetParent(itemsParent.transform, false);
            itemGO.transform.SetParent(transform, false);
            //itemGO.GetComponent<Toggle>().group = itemsParent.GetComponent<ToggleGroup>();
            itemGO.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();

            GameObject grayScaleGO = GameObject.Find("GrayScale");
            grayScaleGO.name += i;

            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);

            Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
            ItemDisplay itemUI = itemGO.GetComponent<ItemDisplay>();

            itemUI.grayScale = grayScaleGO;

            //int itemCount = items.Count(it => it.Name == item.Name);
            int itemCount = 1;

            itemUI.ChangeGrayScale(itemCount > 0 ? false : true);

            itemUI.ChangeItemCounter(itemCount);
            itemUI.ChangeItemImage(item.Icon);
        }
        //RefreshUI();
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