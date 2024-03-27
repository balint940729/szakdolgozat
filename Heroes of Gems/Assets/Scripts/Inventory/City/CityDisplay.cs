using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityDisplay : MonoBehaviour {
    [SerializeField] private GameObject parent = default;
    [SerializeField] private GameObject titlePrefab = default;
    private GameObject title;

    private void Start() {
        CreateTitle("City");
    }

    private void CreateTitle(string titleName) {
        title = Instantiate(titlePrefab);
        title.name = titleName + "Title";
        title.transform.SetParent(parent.transform, false);
        title.GetComponent<Toggle>().interactable = false;

        RectTransform rectTR = (RectTransform)title.transform;
        float titleHeight = rectTR.rect.height;
        title.transform.position = new Vector3(Screen.width * (50f / 100), Screen.height - titleHeight);

        TMP_Text titleText = title.GetComponentInChildren<TMP_Text>();
        titleText.text = titleName;
    }

    public void ChangeTitle(string newTitle) {
        title.GetComponentInChildren<TMP_Text>().text = newTitle;
    }
}