using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class InventoryUI : MonoBehaviour {
    public static bool gameIsPaused = false;
    public GameObject inventoryMenuUI;
    [SerializeField] private GameObject titlePrefab = default;
    public List<GameObject> inventoryContainers;

    private void Start() {
        //EventSystem.current.currentInputModule.de

        CreateTitle("Units", 1);
        CreateTitle("Items", 2);
        CreateTitle("Teams", 3);

        Toggle initialTitle = GameObject.Find("UnitsTitle").GetComponent<Toggle>();
        initialTitle.Select();
        initialTitle.isOn = true;
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (gameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
        
    }

    public void AddInvContainer(GameObject invContainer) {
        inventoryContainers.Add(invContainer);
    }

    private void OnTitleSelected() {
        if (inventoryMenuUI.activeSelf) {
            Toggle activeTitle = GameObject.Find("Inventory").GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();
            if (activeTitle.name.Contains("Units")) {
                List<GameObject> inactive = inventoryContainers.FindAll(cont => !cont.name.Contains("Units"));
                foreach (GameObject inact in inactive) {
                    inact.SetActive(false);
                }
                GameObject container = inventoryContainers.Find(cont => cont.name.Contains("Units"));
                container.SetActive(true);

            }
            else if (activeTitle.name.Contains("Items")) {
                List<GameObject> inactive = inventoryContainers.FindAll(cont => !cont.name.Contains("Items"));
                foreach (GameObject inact in inactive) {
                    inact.SetActive(false);
                }
                GameObject container = inventoryContainers.Find(cont => cont.name.Contains("Items"));
                container.SetActive(true);
            }
        }
    }

    private void CreateTitle(string titleName, int titleCount) {
        GameObject title = Instantiate(titlePrefab);
        title.name = titleName + "Title";
        title.transform.SetParent(inventoryMenuUI.transform, false);
        title.GetComponent<Toggle>().group = inventoryMenuUI.GetComponent<ToggleGroup>();

        RectTransform rectTR = (RectTransform)title.transform;
        float titleHeight = rectTR.rect.height;
        title.transform.position = new Vector3(Screen.width * (25f * titleCount) / 100, Screen.height - titleHeight);

        TMP_Text titleText = title.GetComponentInChildren<TMP_Text>();
        titleText.text = titleName;

        title.GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            OnTitleSelected();
        });

        //if (titleName == "Units") {
        //    title.GetComponent<Toggle>().Select();
        //    title.GetComponent<Toggle>().isOn = true;
        //}
    }

    public void Resume() {
        inventoryMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause() {
        inventoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}