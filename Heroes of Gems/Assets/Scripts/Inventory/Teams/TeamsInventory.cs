using System.Collections;
using System.Collections.Generic;
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
        SelectTeam();
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
        int teamC = Teams.GetTeams().Count;

        for (int i = 0; i < (teamC == 0 ? 1 : teamC); i++) {
            AddTeamSlot();
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

    private void AddTeamSlot() {
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

                if (teamSlotsGO.transform.childCount == 0) {
                    childTR.gameObject.GetComponent<Toggle>().isOn = true;
                    GetComponentInParent<InventoryUI>().AddSelectedTeamButton(itemGO.GetComponent<Team>());
                }

                teamSlotsSelection.Add(childTR.gameObject);
                break;
            }
        }
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
            AddTeamSlot();
        }
    }

    private void SelectTeam() {
        foreach (GameObject teamSlotButton in teamSlotsSelection) {
            if (teamSlotButton.GetComponent<Toggle>().isOn) {
                teamSlotButton.GetComponent<Toggle>().image.sprite = teamSlotButton.GetComponent<Toggle>().spriteState.selectedSprite;
                GetComponentInParent<InventoryUI>().AddSelectedTeamButton(teamSlotButton.GetComponentInParent<Team>());

                //teamSlotButton.GetComponentInParent<Team>().SetSelected(true);
                List<TeamObjectData> asd = Teams.GetTeams().FindAll(t => t.teamName != teamSlotButton.GetComponentInParent<Team>().gameObject.name);
                foreach (TeamObjectData team in Teams.GetTeams()) {
                    if (team.teamName == teamSlotButton.GetComponentInParent<Team>().gameObject.name) {
                        team.isSelected = true;
                    }
                    else {
                        team.isSelected = false;
                    }
                }
            }
            else {
                teamSlotButton.GetComponentInParent<Team>().SetSelected(true);
                teamSlotButton.GetComponent<Toggle>().image.sprite = teamSlotButton.GetComponent<Toggle>().spriteState.disabledSprite;
            }
        }
    }
}