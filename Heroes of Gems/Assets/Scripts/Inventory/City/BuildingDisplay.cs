using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour {
    public Building building;
    [SerializeField] private Image image = default;
    [SerializeField] private TMP_Text buildingText = default;
    [SerializeField] private TMP_Text upgradeText = default;

    public void Start() {
        buildingText.text = building.buildingName;
        upgradeText.text = $"Upgrade ({building.upgradeCost}g)";
    }

    public void ChangeImage(Sprite sprite) {
        image.sprite = sprite;
    }

    public void ChangeUpgradeButtonText(string buttonText) {
        upgradeText.text = buttonText;
    }

    public void ChangeUpgradeButtonText(int upgradeCost) {
        upgradeText.text = $"Upgrade ({upgradeCost}g)";
    }
}