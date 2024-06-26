﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    [SerializeField] private GameObject inventoryMenuUI = default;
    [SerializeField] private GameObject titlePrefab = default;
    [SerializeField] private GameObject closeButton = default;
    [SerializeField] private GameObject gold = default;
    public List<GameObject> inventoryContainers;

    private List<GameObject> titlesGO = new List<GameObject>();

    private void Start() {
        closeButton.GetComponentInChildren<Button>().onClick.AddListener(ShowInventory);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            ShowInventory();
        }
    }

    private void LateUpdate() {
        foreach (GameObject title in titlesGO) {
            if (title.GetComponent<Toggle>().isOn) {
                title.GetComponent<Toggle>().image.sprite = title.GetComponent<Toggle>().spriteState.selectedSprite;
            }
            else {
                title.GetComponent<Toggle>().image.sprite = title.GetComponent<Toggle>().spriteState.disabledSprite;
            }
        }
    }

    private void ShowInventory() {
        if (PauseStateHandler.IsGamePaused()) {
            Resume();
        }
        else {
            Pause();
        }
    }

    public void InitialTitles() {
        titlesGO.Add(CreateTitle("Units", 1));
        titlesGO.Add(CreateTitle("Items", 3));

        InitialSelection();
        OnTitleSelected();
    }

    public void AddInvContainer(GameObject invContainer) {
        inventoryContainers.Add(invContainer);
    }

    private void OnTitleSelected() {
        if (inventoryMenuUI.activeSelf) {
            Toggle activeTitle = inventoryMenuUI.GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault();
            if (activeTitle.name.Contains("Units")) {
                List<GameObject> inactive = inventoryContainers.FindAll(cont => !cont.name.Contains("Teams") || !cont.name.Contains("Units"));
                foreach (GameObject inact in inactive) {
                    inact.SetActive(false);
                }
                List<GameObject> containers = inventoryContainers.FindAll(cont => cont.name.Contains("Teams") || cont.name.Contains("Units"));
                foreach (GameObject container in containers) {
                    container.SetActive(true);
                }
            }
            else if (activeTitle.name.Contains("Items")) {
                List<GameObject> inactive = inventoryContainers.FindAll(cont => !cont.name.Contains("Items") || cont.name.Contains("Equipments"));
                foreach (GameObject inact in inactive) {
                    inact.SetActive(false);
                }
                List<GameObject> containers = inventoryContainers.FindAll(cont => cont.name.Contains("Items") || cont.name.Contains("Equipments"));
                foreach (GameObject container in containers) {
                    container.SetActive(true);
                }
            }
        }
    }

    private GameObject CreateTitle(string titleName, int titleCount) {
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

        return title;
    }

    public void InitialSelection() {
        Toggle initialTitle = titlesGO.Find(title => title.name.Contains("Units")).GetComponent<Toggle>();
        initialTitle.Select();
        initialTitle.isOn = true;
    }

    public void AddSelectedTeamButton(Team team) {
        PlayerTeamHandler.SetTeam(team.GetTeam());
    }

    private void Resume() {
        inventoryMenuUI.SetActive(false);
        closeButton.SetActive(false);
        gold.SetActive(false);
        Time.timeScale = 1f;
        PauseStateHandler.SetGamePause(false);
    }

    private void Pause() {
        inventoryMenuUI.SetActive(true);
        closeButton.SetActive(true);
        gold.SetActive(true);
        Time.timeScale = 0f;
        PauseStateHandler.SetGamePause(true);
    }
}