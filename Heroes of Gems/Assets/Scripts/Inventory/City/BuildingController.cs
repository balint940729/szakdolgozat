using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour {
    [SerializeField] private GameObject upgradeButton = default;
    private BuildingDisplay buildingUI;
    private readonly string folderPath = "Assets/Sprites/Buildings";
    private string[] assetGuids;

    private int level;

    private int maxLevel;
    private Sprite[] images;
    private List<ModifStats> bonusStats;
    private List<Race> affectedRaces;
    private BuildignBonus baseBonus;
    private int bonusModifier;
    private int upgradeCost;

    private static List<BuildingController> activeBuildings = new List<BuildingController>();

    private void Awake() {
        buildingUI = GetComponent<BuildingDisplay>();
    }

    public void SetUpBuilding(Building building) {
        assetGuids = AssetDatabase.FindAssets(building.buildingName + " t:Building", new string[] { folderPath });
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
        buildingUI.building = AssetDatabase.LoadAssetAtPath<Building>(assetPath);

        level = buildingUI.building.buildingLevel;
        images = buildingUI.building.images;
        maxLevel = images.Length - 1;

        baseBonus = buildingUI.building.buildingBonus;
        bonusStats = buildingUI.building.buildingBonus.bonusStats;
        bonusModifier = buildingUI.building.buildingBonus.bonusModifier;
        affectedRaces = buildingUI.building.buildingBonus.affectedRaces;

        upgradeCost = buildingUI.building.upgradeCost;

        upgradeButton.GetComponent<Button>().onClick.AddListener(UpgradeBuilding);
    }

    private void UpgradeBuilding() {
        if (level <= maxLevel && GoldController.HasEnoughGold(upgradeCost)) {
            level++;
            buildingUI.ChangeImage(images[level]);
            GoldController.SpendGold(upgradeCost);

            if (level == 1) {
                activeBuildings.Add(this);
            }

            if (level == maxLevel) {
                SetUpgradeButtonActive(false);
            }
        }
    }

    private void SetUpgradeButtonActive(bool isActive) {
        upgradeButton.GetComponent<Button>().interactable = isActive;
    }

    public List<Race> GetAffectedRaces() {
        return affectedRaces;
    }

    public int GetTotalBonusModify() {
        return level * bonusModifier;
    }

    public List<ModifStats> GetBonusStats() {
        return bonusStats;
    }

    public int GetLevel() {
        return level;
    }

    public static List<BuildingController> GetActiveBuildings() {
        return activeBuildings;
    }

    public BuildignBonus GetBonus() {
        return baseBonus;
    }
}