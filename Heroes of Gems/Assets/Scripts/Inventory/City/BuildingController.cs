using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour {
    [SerializeField] private GameObject upgradeButton = default;
    private BuildingDisplay buildingUI;
    private readonly string folderPath = "Assets/Sprites/Buildings";
    private string[] assetGuids;

    //private Building building;
    private int level;

    private int maxLevel;
    private Sprite[] images;
    private List<RaceStats> bonusStats;
    private int bonusModifier;

    private void Awake() {
        buildingUI = GetComponent<BuildingDisplay>();
    }

    //public BuildingController(Building building) {
    //    this.building = building;
    //    level = building.buildingLevel;
    //    maxLevel = building.buildingMaxLevel;
    //    images = building.images;
    //    bonusStats = building.bonusStats;
    //    bonusModifier = building.bonusModifier;
    //}

    public void SetUpBuilding(Building building) {
        assetGuids = AssetDatabase.FindAssets(building.buildingName + " t:Building", new string[] { folderPath });
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
        buildingUI.building = AssetDatabase.LoadAssetAtPath<Building>(assetPath);

        level = buildingUI.building.buildingLevel;
        maxLevel = buildingUI.building.buildingMaxLevel;
        images = buildingUI.building.images;
        bonusStats = buildingUI.building.bonusStats;
        bonusModifier = buildingUI.building.bonusModifier;

        upgradeButton.GetComponent<Button>().onClick.AddListener(UpgradeBuilding);
    }

    public void SetUpUpgradeButton(GameObject buttonGO) {
        upgradeButton = buttonGO;
        upgradeButton.GetComponent<Button>().onClick.AddListener(UpgradeBuilding);
    }

    public void SetUpUpgradeButton() {
        upgradeButton.GetComponent<Button>().onClick.AddListener(UpgradeBuilding);
    }

    private void UpgradeBuilding() {
        if (level < maxLevel) {
            level++;
            buildingUI.ChangeImage(images[level]);

            if ((level + 1) == maxLevel) {
                SetUpgradeButtonActive(false);
            }
        }
    }

    private void SetUpgradeButtonActive(bool isActive) {
        upgradeButton.GetComponent<Button>().interactable = isActive;
    }

    public int GetTotalBonusModify() {
        return (level == 0 ? 1 : level) * bonusModifier;
    }

    public List<RaceStats> GetBonusStats() {
        return bonusStats;
    }
}