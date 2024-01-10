using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemsInventory : BaseInventory {

    private void Start() {
        FillInventory();
    }

    protected override void FillInventory() {
        //if (itemsParent != null) {
        //    itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        //}

        assetGuids = AssetDatabase.FindAssets("t:Item", new string[] { folderPath });
        for (int i = 0; i < assetGuids.Length; i++) {
            GameObject itemGO = Instantiate(emptyItemPrefab);
            itemGO.name = "ItemSlot" + i;
            itemGO.transform.SetParent(transform, false);
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
    }
}