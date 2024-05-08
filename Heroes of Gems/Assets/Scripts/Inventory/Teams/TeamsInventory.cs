using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamsInventory : BaseInventory {
    private GameObject teamSlotsGO;
    private GameObject buttonGO;
    private GameObject buttonsContentGO;
    private List<GameObject> teamSlotsSelection = new List<GameObject>();

    private void Start() {
        FillInventory();
        LoadSelection();
    }

    private void LateUpdate() {
        if (Input.GetMouseButtonUp(0)) {
            SelectTeam();
        }
    }

    protected override void FillInventory() {
        teamSlotsGO = Instantiate(otherPrefabs.Find(other => other.name == "InventoryContent"));
        teamSlotsGO.name = "TeamSlotsContent";
        teamSlotsGO.transform.SetParent(transform, false);
        StartCoroutine(ChangeTeamSlotLayoutGroup(teamSlotsGO));

        if (MainMenuScript.isNewGame) {
            GameObject teamSlot = AddTeamSlot();
            Teams.AddTeam(teamSlot.GetComponent<Team>());
        }
        else {
            for (int i = 0; i < Teams.GetTeams().Count; i++) {
                GameObject teamSlot = AddTeamSlot();
                LoadTeamUnits(teamSlot);
            }
        }

        AddTeamSlotButton();
    }

    private IEnumerator ChangeTeamSlotLayoutGroup(GameObject content) {
        GridLayoutGroup deleteGrid = content.GetComponent<GridLayoutGroup>();

        Destroy(deleteGrid);
        yield return null;
        content.AddComponent<VerticalLayoutGroup>();
        VerticalLayoutGroup verticalGroup = content.GetComponent<VerticalLayoutGroup>();

        verticalGroup.spacing = 325f;
        verticalGroup.padding = new RectOffset(0, 0, 170, 170);

        ContentSizeFitter sizeFitter = content.GetComponent<ContentSizeFitter>();
        Destroy(sizeFitter);
    }

    private IEnumerator ChangeButtonLayoutGroup(GameObject content) {
        GridLayoutGroup deleteGrid = content.GetComponent<GridLayoutGroup>();

        Destroy(deleteGrid);
        yield return null;
        content.AddComponent<VerticalLayoutGroup>();
        VerticalLayoutGroup verticalGroup = content.GetComponent<VerticalLayoutGroup>();

        verticalGroup.childAlignment = TextAnchor.UpperCenter;
        ContentSizeFitter sizeFitter = content.GetComponent<ContentSizeFitter>();
        Destroy(sizeFitter);

        verticalGroup.padding = new RectOffset(0, 0, -40, 0);
    }

    private GameObject AddTeamSlot() {
        GameObject itemGO = Instantiate(emptyItemPrefab);

        itemGO.name = "Team" + teamSlotsGO.transform.childCount;
        itemGO.transform.SetParent(teamSlotsGO.transform, false);
        itemGO.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
        itemGO.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

        //Teams.AddTeam(itemGO.GetComponent<Team>());

        foreach (Transform childTR in itemGO.transform) {
            if (childTR.tag == "TeamSelectButton") {
                childTR.gameObject.name += teamSlotsGO.transform.childCount;
                childTR.gameObject.GetComponent<Toggle>().group = teamSlotsGO.GetComponent<ToggleGroup>();

                teamSlotsSelection.Add(childTR.gameObject);
                break;
            }
        }

        return itemGO;
    }

    private void AddTeamSlotButton() {
        buttonsContentGO = Instantiate(otherPrefabs.Find(other => other.name == "InventoryContent"));
        buttonsContentGO.name = "ButtonsContent";
        buttonsContentGO.transform.SetParent(transform, false);
        StartCoroutine(ChangeButtonLayoutGroup(buttonsContentGO));

        buttonGO = Instantiate(otherPrefabs.Find(other => other.name == "ButtonPrefab"));
        buttonGO.name = "BuyTeamButton";
        buttonGO.transform.SetParent(buttonsContentGO.transform, false);
        buttonGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        buttonGO.GetComponent<Button>().onClick.AddListener(BuyTeamSlot);

        buttonGO.GetComponentInChildren<TMP_Text>().enableAutoSizing = true;
        buttonGO.GetComponentInChildren<TMP_Text>().enableWordWrapping = false;
        buttonGO.GetComponentInChildren<TMP_Text>().fontSizeMin = 18;
        buttonGO.GetComponentInChildren<TMP_Text>().fontSizeMax = 28;
        buttonGO.GetComponentInChildren<TMP_Text>().text = "Buy Teamslot (50g)";
    }

    private void BuyTeamSlot() {
        if (GoldController.HasEnoughGold(50)) {
            GoldController.SpendGold(50);
            GameObject teamSlot = AddTeamSlot();
            Teams.AddTeam(teamSlot.GetComponent<Team>());
        }
    }

    private void LoadTeamUnits(GameObject teamSlots) {
        //When loaded
        TeamObjectData team = Teams.GetTeams().Find(t => t.teamName == teamSlots.name);

        teamSlots.GetComponent<Team>().SetTeam(team.members);

        UnitItem[] teamSlotUnits = teamSlots.GetComponentsInChildren<UnitItem>();
        TeamSlotDisplay[] teamSlotDisplays = teamSlots.GetComponentsInChildren<TeamSlotDisplay>();
        DropUnit[] dropUnits = teamSlots.GetComponentsInChildren<DropUnit>();

        //Assume it will be the same length both
        for (int j = 0; j < teamSlotUnits.Length; j++) {
            teamSlotUnits[j].unit = team.members[j];
            if (teamSlotUnits[j].unit != null) {
                teamSlotDisplays[j].SetMemberDisplay(teamSlotUnits[j].unit);
                dropUnits[j].AddDragUnit();
            }
            else {
                teamSlotDisplays[j].ResetTeamSlotDisplay();
            }
        }
    }

    private void LoadSelection() {
        //When loaded
        if (Teams.GetTeams().Count > 0) {
            for (int i = 0; i < Teams.GetTeams().Count; i++) {
                GameObject teamSlotButton = teamSlotsSelection.ElementAt(i);
                TeamObjectData team = Teams.GetTeams().ElementAt(i);

                if (team.isSelected || Teams.GetTeams().Count == 1) {
                    teamSlotButton.GetComponent<Toggle>().isOn = true;
                    teamSlotButton.GetComponentInParent<Team>().SetSelected(true);
                    teamSlotButton.GetComponent<Toggle>().image.sprite = teamSlotButton.GetComponent<Toggle>().spriteState.selectedSprite;
                    GetComponentInParent<InventoryUI>().AddSelectedTeamButton(teamSlotButton.GetComponentInParent<Team>());
                }
                else {
                    teamSlotButton.GetComponent<Toggle>().isOn = false;
                    teamSlotButton.GetComponentInParent<Team>().SetSelected(false);
                    teamSlotButton.GetComponent<Toggle>().image.sprite = teamSlotButton.GetComponent<Toggle>().spriteState.disabledSprite;
                }
            }
        }
    }

    private void SelectTeam() {
        foreach (GameObject teamSlotButton in teamSlotsSelection) {
            if (teamSlotButton.GetComponent<Toggle>().isOn) {
                teamSlotButton.GetComponent<Toggle>().image.sprite = teamSlotButton.GetComponent<Toggle>().spriteState.selectedSprite;
                GetComponentInParent<InventoryUI>().AddSelectedTeamButton(teamSlotButton.GetComponentInParent<Team>());
                teamSlotButton.GetComponentInParent<Team>().SetSelected(true);
                Teams.GetTeams().First(t => t.teamName == teamSlotButton.GetComponentInParent<Team>().name).isSelected = true;
            }
            else {
                teamSlotButton.GetComponentInParent<Team>().SetSelected(false);
                Teams.GetTeams().First(t => t.teamName == teamSlotButton.GetComponentInParent<Team>().name).isSelected = false;
                teamSlotButton.GetComponent<Toggle>().image.sprite = teamSlotButton.GetComponent<Toggle>().spriteState.disabledSprite;
            }
        }
    }
}