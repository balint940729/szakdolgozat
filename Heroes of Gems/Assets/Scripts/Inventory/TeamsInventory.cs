using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamsInventory : BaseInventory {
    private GameObject teamSlotsGO;
    private GameObject buttonGO;
    private GameObject buttonsContentGO;

    private void Start() {
        FillInventory();
    }

    protected override void FillInventory() {
        teamSlotsGO = Instantiate(otherPrefabs.Find(other => other.name == "InventoryContent"));
        teamSlotsGO.name = "TeamSlotsContent";
        teamSlotsGO.transform.SetParent(transform, false);
        StartCoroutine(ChangeTeamSlotLayoutGroup(teamSlotsGO));
        teamSlotsGO.AddComponent<Teams>();
        teamSlotsGO.GetComponent<Teams>().teams = new List<Team>();

        for (int i = 0; i < 2; i++) {
            GameObject itemGO = Instantiate(emptyItemPrefab);
            itemGO.name = "Team" + i;
            itemGO.transform.SetParent(teamSlotsGO.transform, false);
            itemGO.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
            itemGO.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
            teamSlotsGO.GetComponent<Teams>().teams.Add(itemGO.GetComponent<Team>());
        }

        buttonsContentGO = Instantiate(otherPrefabs.Find(other => other.name == "InventoryContent"));
        buttonsContentGO.name = "ButtonsContent";
        buttonsContentGO.transform.SetParent(transform, false);
        StartCoroutine(ChangeButtonLayoutGroup(buttonsContentGO));

        buttonGO = Instantiate(otherPrefabs.Find(other => other.name == "ButtonPrefab"));
        buttonGO.name = "BuyTeamButton";
        buttonGO.transform.SetParent(buttonsContentGO.transform, false);
        buttonGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        buttonGO.GetComponent<Button>().onClick.AddListener(AddTeamSlot);

        buttonGO.GetComponentInChildren<TMP_Text>().enableAutoSizing = true;
        buttonGO.GetComponentInChildren<TMP_Text>().enableWordWrapping = false;
        buttonGO.GetComponentInChildren<TMP_Text>().fontSizeMin = 18;
        buttonGO.GetComponentInChildren<TMP_Text>().fontSizeMax = 28;
        buttonGO.GetComponentInChildren<TMP_Text>().text = "Buy Teamslot (200g)";
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
        teamSlotsGO.GetComponent<Teams>().teams.Add(itemGO.GetComponent<Team>());

        //GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(buttonsContentGO.transform.position.x, buttonsContentGO.transform.position.y);
    }
}