using UnityEngine;
using UnityEngine.UI;

public class EquipmentDisplay : MonoBehaviour {
    [SerializeField] private Image equipmentImage = default;
    [SerializeField] private Sprite defaultImage = default;

    private void Awake() {
        equipmentImage.sprite = defaultImage;
        Color tempColor = equipmentImage.color;
        tempColor.a = 0.5f;
        equipmentImage.color = tempColor;
    }

    public void SetEquipmentDisplay(Item item) {
        equipmentImage.sprite = item.itemIcon;
        Color tempColor = equipmentImage.color;
        tempColor.a = 1f;
        equipmentImage.color = tempColor;
    }

    public void ResetEquipmentDisplay() {
        equipmentImage.sprite = defaultImage;
        Color tempColor = equipmentImage.color;
        tempColor.a = 0.5f;
        equipmentImage.color = tempColor;
    }
}