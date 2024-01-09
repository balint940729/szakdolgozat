using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UnitsInventory : BaseInventory {
    //public GameObject emptyItemPrefab;

    //[SerializeField] private List<Item> items;

    //public Transform itemsParent;
    //public ItemSlot[] itemSlots;

    ////public int numberOfItems;
    //public string folderPath = "Assets/Sprites/Cards";

    //private string[] assetGuids;


    // Start is called before the first frame update
    private void Start() {
        FillInventory();
    }

    protected override void FillInventory() {
        //if (itemsParent != null) {
        //    itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        //}

        assetGuids = AssetDatabase.FindAssets("t:Unit", new string[] { folderPath });
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

            Unit item = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);
            ItemDisplay itemUI = itemGO.GetComponent<ItemDisplay>();

            itemUI.grayScale = grayScaleGO;

            //int itemCount = items.Count(it => it.Name == item.Name);
            int itemCount = 0;

            itemUI.ChangeGrayScale(itemCount > 0 ? false : true);

            itemUI.ChangeItemCounter(itemCount);
            itemUI.ChangeItemImage(item.image);
        }
        //RefreshUI();
    }




 
}