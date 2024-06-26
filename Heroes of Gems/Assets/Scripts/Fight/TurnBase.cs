﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnBase : MonoBehaviour {

    // Card Prefab - Border, Attack, Health, Armor, Mana icons
    [SerializeField] private GameObject cardPrefab = default;

    [SerializeField] private GameObject spellPrefab = default;
    [SerializeField] private GameObject buttonPrefab = default;
    [SerializeField] private GameObject turnPrefab = default;
    [SerializeField] private GameObject parentScene = default;

    private Transform parentSceneTR;
    private GameObject turnArrowGO = default;

    private List<GameObject> playerTeam = new List<GameObject>();
    private List<GameObject> playerTeamSpells = new List<GameObject>();
    private List<GameObject> enemyTeam = new List<GameObject>();
    private List<GameObject> enemyTeamSpells = new List<GameObject>();
    private List<GameObject> castButtons = new List<GameObject>();
    private static TurnBase instance;

    private BattleState state;

    private const string ally = "Ally";
    private const string enemy = "Enemy";

    private float arrowX;
    private float arrowY;

    public static TurnBase GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        FindObjectOfType<Match3>().attackTriggered += Combat;
        FindObjectOfType<Match3>().gainManaTriggered += GainMana;
        FindObjectOfType<Match3>().turnChangeTriggered += TurnChange;

        arrowX = (Screen.width * 11.5f / 100);
        arrowY = (Screen.height * 98 / 100);

        SetUpTeam(true);
        SetUpTeam(false);

        CheckStatBonuses(playerTeam, true);
        CheckStatBonuses(enemyTeam, false);

        SetUpBoard();
    }

    private void CheckStatBonuses(List<GameObject> team, bool isPlayer) {
        RaceBonus.InitializeBonus(team);

        if (isPlayer) {
            EquipmentBonus.InitializeBonus(team);
            BuildingBonus.InitializeBonus(team);
        }
    }

    private void SpellDisplay(string spellName) {
        GameObject displayedSpell = playerTeamSpells.Where(obj => obj.name == spellName).SingleOrDefault();
        if (displayedSpell == null) {
            displayedSpell = enemyTeamSpells.Where(obj => obj.name == spellName).SingleOrDefault();
        }

        string unitName = spellName.Replace("Spell", "");

        GameObject displayedUnit = playerTeam.Where(obj => obj.name == unitName).SingleOrDefault();
        if (displayedUnit == null) {
            displayedUnit = enemyTeam.Where(obj => obj.name == unitName).SingleOrDefault();
        }

        if (displayedSpell != null && displayedUnit != null) {
            GameObject castButton = castButtons.Where(obj => obj.name.Contains(unitName)).SingleOrDefault();
            UnitController unit = displayedUnit.GetComponent<UnitController>();

            BattleState state = (unitName.Contains(ally)) ? BattleState.WaitingForPlayer : BattleState.WaitingForEnemy;

            if (unit.IsOnFullMana() && BattleStateHandler.GetState() == state) {
                castButton.GetComponent<Button>().interactable = true;
            }
            else {
                castButton.GetComponent<Button>().interactable = false;
            }

            SpellController.ShowSpell(displayedSpell, displayedUnit);
        }
    }

    public void CastSpell(UnitController player) {
        if (BattleStateHandler.GetState() != BattleState.Won && BattleStateHandler.GetState() != BattleState.Lost) {
            bool isSpellCasted = player.CastSpell();
            if (isSpellCasted) {
                if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
                    BattleStateHandler.SetState(BattleState.WaitingForEnemy);
                }
                else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
                    BattleStateHandler.SetState(BattleState.WaitingForPlayer);
                }
                TurnChange();
            }
            RemoveOnZero();
        }
    }

    private void TurnChange() {
        state = BattleStateHandler.GetState();
        if (state != BattleState.Won && state != BattleState.Lost) {
            if (state == BattleState.WaitingForPlayer) {
                arrowX = (Screen.width * 11.5f / 100);
                turnArrowGO.transform.position = new Vector3(arrowX, arrowY);
            }
            else if (state == BattleState.WaitingForEnemy) {
                arrowX = (Screen.width * 88.5f / 100);
                turnArrowGO.transform.position = new Vector3(arrowX, arrowY);
            }
        }
    }

    private void GainMana(int manaAmount, string color) {
        state = BattleStateHandler.GetState();

        if ((state != BattleState.Won && state != BattleState.Lost) && color != null) {
            List<GameObject> team = playerTeam;
            int extraMana = 0;
            int manaGained = manaAmount;

            if (state == BattleState.PlayerTurn) {
                team = playerTeam;
            }
            else if (state == BattleState.EnemyTurn) {
                team = enemyTeam;
            }

            foreach (GameObject unit in team) {
                UnitController unitCntr = unit.GetComponent<UnitController>();

                if (unitCntr.IsOnFullMana()) {
                    continue;
                }

                List<Colors> unitColors = unitCntr.GetColors();

                if (unitColors.Find(Colors => Colors.colorName == color) != null) {
                    extraMana += unitCntr.GainMana(manaGained);

                    if (extraMana == 0) {
                        break;
                    }

                    manaGained = extraMana;
                    extraMana = 0;
                }
            }
        }
    }

    private void Combat() {
        state = BattleStateHandler.GetState();
        if (state != BattleState.Won && state != BattleState.Lost) {
            UnitController player = playerTeam[0].GetComponent<UnitController>();
            UnitController enemy = enemyTeam[0].GetComponent<UnitController>();

            if (state == BattleState.PlayerTurn) {
                player.SkullDamage(player.GetAttack(), enemy);
            }
            else if (state == BattleState.EnemyTurn) {
                enemy.SkullDamage(enemy.GetAttack(), player);
            }
            RemoveOnZero();
        }
    }

    private void SetUpBoard() {
        turnArrowGO = Instantiate(turnPrefab);

        turnArrowGO.name = "TurnArrow";
        turnArrowGO.transform.SetParent(parentSceneTR.transform, false);

        turnArrowGO.transform.position = new Vector3(arrowX, arrowY);
        turnArrowGO.transform.SetAsLastSibling();
    }

    // Setup the Card to the Board
    private void SetUpTeam(bool isPlayerTeam) {
        Unit[] tempTeamList;
        parentSceneTR = parentScene.transform;

        if (isPlayerTeam) {
            tempTeamList = PlayerTeamHandler.GetTeam();
        }
        else {
            tempTeamList = EnemyTeamHandler.GetTeam();
        }

        for (int i = 0; i < tempTeamList.Length; i++) {
            if (tempTeamList[i] != null) {
                SetUpUnit(isPlayerTeam, i, tempTeamList[i]);
            }
        }
    }

    private void SetUpUnit(bool isPlayerTeam, int index, Unit card) {
        GameObject cardUnitGO = Instantiate(cardPrefab);
        GameObject spellGO = Instantiate(spellPrefab);

        float cardPos = (isPlayerTeam) ? 11.5f : 88.5f;
        string teamName = (isPlayerTeam) ? ally : enemy;

        float cardX = (Screen.width * cardPos / 100);
        float cardY = (Screen.height * (87.5f - index * 25) / 100);

        cardUnitGO.name = teamName + index;
        cardUnitGO.transform.SetParent(parentSceneTR.transform, false);
        cardUnitGO.transform.position = new Vector3(cardX, cardY);

        spellGO.name = teamName + index + "Spell";
        spellGO.transform.SetParent(cardUnitGO.transform, false);
        spellGO.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        spellGO.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

        RectTransform rectTr = (RectTransform)spellGO.transform;
        float spellGOWidth = rectTr.rect.width;
        float spellGOHeight = rectTr.rect.height;

        Vector3 cancelButtonPosition = new Vector3(spellGO.transform.position.x - (spellGOWidth / 2), spellGO.transform.position.y - (spellGO.transform.position.y - spellGOHeight / 1.7f));
        Vector3 castButtonPosition = new Vector3(spellGO.transform.position.x + (spellGOWidth / 2), spellGO.transform.position.y - (spellGO.transform.position.y - spellGOHeight / 1.7f));

        GameObject castButton = CreateButton(teamName + index + "CastButton", "Cast", spellGO, castButtonPosition);
        castButton.GetComponent<Button>().onClick.AddListener(castButton.GetComponent<SpellController>().CastSpell);
        castButtons.Add(castButton);

        GameObject cancelButton = CreateButton(teamName + index + "CancelButton", "Cancel", spellGO, cancelButtonPosition);
        cancelButton.GetComponent<Button>().onClick.AddListener(SpellController.CloseSpell);

        UnitController cardUnitController = cardUnitGO.GetComponent<UnitController>();

        cardUnitController.SetUp(card, spellGO);

        SetUpUnitColor(isPlayerTeam, cardUnitController, index);

        if (isPlayerTeam) {
            playerTeam.Add(cardUnitGO);
            playerTeamSpells.Add(spellGO);
        }
        else {
            enemyTeam.Add(cardUnitGO);
            enemyTeamSpells.Add(spellGO);
        }

        spellGO.SetActive(false);
        FindObjectOfType<UnitController>().OnSpellDisplay += SpellDisplay;
    }

    private GameObject CreateButton(string buttonName, string buttonText, GameObject parent, Vector3 position) {
        GameObject buttonGO = Instantiate(buttonPrefab);
        buttonGO.name = buttonName;
        buttonGO.transform.SetParent(parent.transform.GetChild(0), false);
        buttonGO.transform.position = position;
        buttonGO.GetComponentInChildren<TMP_Text>().text = buttonText;
        return buttonGO;
    }

    private void SetUpUnitColor(bool isPlayerTeam, UnitController cardUnitController, int index) {
        GameObject manaBGOriginal = GameObject.Find("ManaBG");
        GameObject bgAll = GameObject.Find("BGImage");
        string unitTeamName;

        if (isPlayerTeam) {
            unitTeamName = ally;
        }
        else {
            unitTeamName = enemy;
        }

        bgAll.name = unitTeamName + "BGImage" + index;

        int tempPrev = manaBGOriginal.transform.GetSiblingIndex();

        for (int j = 0; j < cardUnitController.GetColorsCount(); j++) {
            GameObject manaBG = Instantiate(manaBGOriginal);
            manaBGOriginal.name = unitTeamName + index + "WhiteBG";

            tempPrev++;

            manaBG.name = unitTeamName + index + "ManaBG" + j;
            manaBG.transform.position = new Vector3(-74.0f, 70.0f);
            manaBG.transform.SetParent(bgAll.transform, false);
            manaBG.transform.SetSiblingIndex(tempPrev);

            int temp = cardUnitController.GetColorsCount() - (j + 1);

            cardUnitController.SetUpColors(manaBG, temp);

            foreach (Transform child in manaBG.transform) {
                Destroy(child.gameObject);
            }

            if (j == cardUnitController.GetColorsCount() - 1) {
                foreach (Transform child in manaBGOriginal.transform) {
                    child.transform.SetParent(manaBG.transform, false);
                }
            }
        }
        Destroy(manaBGOriginal.gameObject);
    }

    private void RemoveOnZero() {
        List<GameObject> allUnits = playerTeam.Concat(enemyTeam).ToList();
        List<GameObject> allSpells = playerTeamSpells.Concat(enemyTeamSpells).ToList();

        foreach (GameObject unitGO in allUnits) {
            UnitController unit = unitGO.GetComponent<UnitController>();
            GameObject spellGO;
            GameObject castButtonGO;

            int tempHealth = unit.GetHealth();
            state = BattleStateHandler.GetState();

            if (tempHealth <= 0) {
                if (state != BattleState.Won && state != BattleState.Lost) {
                    if (unitGO.name.Contains(ally)) {
                        spellGO = allSpells.Find(spell => spell.name.Contains(unitGO.name));
                        spellGO = allSpells.Find(spell => spell.name.Contains(unitGO.name));
                        playerTeamSpells.Remove(spellGO);
                        playerTeam.Remove(unitGO);
                    }
                    else if (unitGO.name.Contains(enemy)) {
                        spellGO = allSpells.Find(spell => spell.name.Contains(unitGO.name));
                        enemyTeamSpells.Remove(spellGO);
                        enemyTeam.Remove(unitGO);
                    }

                    castButtonGO = castButtons.Find(btn => btn.name.Contains(unitGO.name));
                    castButtons.Remove(castButtonGO);
                    Destroy(unitGO);
                }
            }
        }

        CheckGameResult();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            BattleStateHandler.SetState(BattleState.Won);
            SceneManager.UnloadSceneAsync(1);
            PauseStateHandler.SetGamePause(false);
            TriggerBattle.enemyGO.GetComponent<Rewards>().GainLoot();
            TriggerBattle.enemyGO.GetComponent<EnemyController>().SetEnemyAlive(false);
            AudioManager.instance.ChangeMusic("Fight", "Theme");
        }
        else if (Input.GetKeyDown(KeyCode.L)) {
            BattleStateHandler.SetState(BattleState.Lost);
            SceneManager.UnloadSceneAsync(1);
            PauseStateHandler.SetGamePause(false);
            AudioManager.instance.ChangeMusic("Fight", "Theme");
        }
    }

    private void CheckGameResult() {
        if (enemyTeam.Count == 0) {
            BattleStateHandler.SetState(BattleState.Won);
            SceneManager.UnloadSceneAsync(1);
            PauseStateHandler.SetGamePause(false);
            TriggerBattle.enemyGO.GetComponent<Rewards>().GainLoot();
            TriggerBattle.enemyGO.GetComponent<EnemyController>().SetEnemyAlive(false);
            AudioManager.instance.ChangeMusic("Fight", "Theme");
        }
        else if (playerTeam.Count == 0) {
            BattleStateHandler.SetState(BattleState.Lost);
            SceneManager.UnloadSceneAsync(1);
            PauseStateHandler.SetGamePause(false);
            AudioManager.instance.ChangeMusic("Fight", "Theme");
        }
    }

    public List<GameObject> GetPlayerTeam() {
        return playerTeam;
    }

    public List<GameObject> GetEnemyTeam() {
        return enemyTeam;
    }
}