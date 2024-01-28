using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour {
    public Building building;
    public Image image;
    public TMP_Text buildingText;

    public void Start() {
        buildingText.text = building.buildingName;
        image.sprite = building.images[0];
    }

    public void ChangeImage(Sprite sprite) {
        image.sprite = sprite;
    }
}