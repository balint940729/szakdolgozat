using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UnitsInventory : BaseInventory {
    [SerializeField] private static List<Unit> units = new List<Unit>();
    [SerializeField] private List<GameObject> unitsGO = new List<GameObject>();
    private List<Unit> testunits = new List<Unit>();
    private static bool invChanged = false;

    private void Start() {
        FillInventory();
    }

    private void Update() {
        if (invChanged) {
            RefreshUI();
            invChanged = false;
        }
    }

    protected override void FillInventory() {
        assetGuids = AssetDatabase.FindAssets("t:Unit", new string[] { folderPath });
        for (int i = 0; i < assetGuids.Length; i++) {
            GameObject itemGO = Instantiate(emptyItemPrefab);
            itemGO.name = "ItemSlot" + i;
            itemGO.transform.SetParent(transform, false);

            GameObject grayScaleGO = GameObject.Find("GrayScale");

            grayScaleGO.name += i;

            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);

            Unit item = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);
            itemGO.AddComponent<UnitItem>();
            itemGO.GetComponent<UnitItem>().unit = item;
            ItemDisplay itemUI = itemGO.GetComponent<ItemDisplay>();

            itemUI.grayScale = grayScaleGO;

            int itemCount = UnitCount(item);

            itemUI.ChangeGrayScale(itemCount > 0 ? false : true);

            itemUI.ChangeItemCounter(itemCount);
            itemUI.ChangeItemImage(item.image);

            if (itemCount > 0) {
                itemGO.transform.GetChild(0).gameObject.AddComponent<DragUnit>();
                itemGO.GetComponentInChildren<DragUnit>().originalGO = itemGO.transform.GetChild(0).gameObject;
                itemGO.GetComponentInChildren<DragUnit>().SetCanvas(inventoryCanvas.GetComponent<Canvas>());
                itemGO.GetComponentInChildren<DragUnit>().SetParent(container);
            }
            else {
                if (itemGO.GetComponentInChildren<DragUnit>() != null) {
                    Destroy(itemGO.GetComponentInChildren<DragUnit>());
                }
            }

            if (item.baseName == "Harpy" || item.baseName == "Gryphon") {
                testunits.Add(item);
            }

            if (UnitsHandler.GetUnits().Count == 0) {
                if (item.baseName == "Dog") {
                    for (int j = 0; j < 4; j++) {
                        units.Add(item);
                        UnitsHandler.AddUnit(item);
                    }
                }
            }



            unitsGO.Add(itemGO);
        }

        GameObject buttonGO = Instantiate(otherPrefabs.Find(other => other.name == "ButtonPrefab"));
        buttonGO.name = "BuyUnitButton";
        buttonGO.GetComponent<Button>().onClick.AddListener(AddUnit2);
        buttonGO.GetComponentInChildren<TMP_Text>().text = "Buy Harpies";
        buttonGO.transform.SetParent(transform, false);

        RefreshUI();
    }

    public void AddUnit2() {
        foreach (Unit unit in testunits) {
            for (int i = 0; i < 1; i++) {
                units.Add(unit);
                UnitsHandler.AddUnit(unit);
            }
        }

        RefreshUI();
    }

    public static void AddUnit(Unit unit) {
        units.Add(unit);
        UnitsHandler.AddUnit(unit);
        invChanged = true;
    }

    public void RemoveUnit(Unit unit) {
        units.Remove(unit);
        RefreshUI();
    }

    protected override void RefreshUI() {
        foreach (GameObject unitGO in unitsGO) {
            ItemDisplay itemUI = unitGO.GetComponent<ItemDisplay>();

            int itemCount = UnitCount(unitGO.GetComponent<UnitItem>().unit);

            itemUI.ChangeGrayScale(itemCount > 0 ? false : true);

            if (itemCount > 0) {
                if (unitGO.transform.GetChild(0).gameObject.GetComponent<DragUnit>() == null) {
                    unitGO.transform.GetChild(0).gameObject.AddComponent<DragUnit>();
                    unitGO.GetComponentInChildren<DragUnit>().originalGO = unitGO.transform.GetChild(0).gameObject;
                    unitGO.GetComponentInChildren<DragUnit>().SetCanvas(inventoryCanvas.GetComponent<Canvas>());
                    unitGO.GetComponentInChildren<DragUnit>().SetParent(container);
                }
            }
            else {
                if (unitGO.GetComponentInChildren<DragUnit>() != null) {
                    Destroy(unitGO.GetComponentInChildren<DragUnit>());
                }
            }

            itemUI.ChangeItemCounter(itemCount);
        }
    }

    public int UnitCount(Unit unit) {
        List<Unit> copy = UnitsHandler.GetUnits();
        //List<Unit> copy = units;
        int asd = copy.Count(it => it.baseName == unit.baseName);
        return asd; ;
    }

    //public void LoadData(GameData gameData) {
    //    units = gameData.units;
    //}

    //public void SaveData(ref GameData gameData) {
    //    gameData.units = units;
    //    gameData.units = testunits;
    //}
}