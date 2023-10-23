using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TurnBase : MonoBehaviour {

    // Card Prefab - Border, Attack, Health, Armor icons
    public GameObject cardPrefab;

    public GameObject spellPrefab;
    public GameObject buttonPrefab;
    public GameObject turnPrefab;

    private Transform parentScene;
    private GameObject turnArrowGO;

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

    // Start is called before the first frame update
    private void Start() {
        FindObjectOfType<Match3>().attackTriggered += Combat;
        FindObjectOfType<Match3>().gainManaTriggered += gainMana;
        FindObjectOfType<Match3>().turnChangeTriggered += TurnChange;

        arrowX = (Screen.width * 11.5f / 100);
        arrowY = (Screen.height * 98 / 100);

        parentScene = GameObject.Find("GameCanvas").transform;

        SetUpTeam(true);
        SetUpTeam(false);

        SetUpBoard();
    }

    //private void Update() {
    //    if (Input.GetKeyDown(KeyCode.Space)) {
    //    }
    //}

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

            if (unit.isOnFullMana() && BattleStateHandler.GetState() == state) {
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
            List<UnitController> targets = new List<UnitController>();
            List<UnitController> allyTargets = new List<UnitController>();

            if (player.name.Contains(ally)) {
                foreach (GameObject gameObject in playerTeam) {
                    allyTargets.Add(gameObject.GetComponent<UnitController>());
                }

                foreach (GameObject gameObject in enemyTeam) {
                    targets.Add(gameObject.GetComponent<UnitController>());
                }
            }
            else if (player.name.Contains(enemy)) {
                foreach (GameObject gameObject in enemyTeam) {
                    allyTargets.Add(gameObject.GetComponent<UnitController>());
                }

                foreach (GameObject gameObject in playerTeam) {
                    targets.Add(gameObject.GetComponent<UnitController>());
                }
            }

            bool isSpellCasted = player.castSpell(allyTargets, targets);
            if (isSpellCasted) {
                if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
                    BattleStateHandler.setState(BattleState.WaitingForEnemy);
                }
                else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
                    BattleStateHandler.setState(BattleState.WaitingForPlayer);
                }
                TurnChange();
            }
            removeOnZero();
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

    private void gainMana(int manaAmount, string color) {
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

                if (unitCntr.isOnFullMana()) {
                    continue;
                }

                List<Colors> unitColors = unitCntr.getColors();

                if (unitColors.Find(Colors => Colors.colorName == color) != null) {
                    extraMana += unitCntr.gainMana(manaGained);

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
                player.normalDamage(player.GetAttack(), enemy);
            }
            else if (state == BattleState.EnemyTurn) {
                enemy.normalDamage(enemy.GetAttack(), player);
            }
            removeOnZero();
        }
    }

    private void SetUpBoard() {
        turnArrowGO = Instantiate(turnPrefab);

        turnArrowGO.name = "TurnArrow";
        turnArrowGO.transform.SetParent(parentScene.transform, false);

        turnArrowGO.transform.position = new Vector3(arrowX, arrowY);
        turnArrowGO.transform.SetAsLastSibling();
    }

    // Setup the Card to the Board
    private void SetUpTeam(bool isPlayerTeam) {
        int[] tempTeamList;
        if (isPlayerTeam) {
            tempTeamList = new int[] { 0, 0, 1, 2 };
        }
        else {
            tempTeamList = new int[] { 1, 2, 1, 1 };
        }
        for (int i = 0; i < 4; i++) {
            SetUpUnit(isPlayerTeam, i, tempTeamList[i]);
        }
    }

    private void SetUpUnit(bool isPlayerTeam, int index, int cardID) {
        GameObject cardUnitGO = Instantiate(cardPrefab);
        GameObject spellGO = Instantiate(spellPrefab);

        float cardPos = (isPlayerTeam) ? 11.5f : 88.5f;
        string teamName = (isPlayerTeam) ? ally : enemy;

        float cardX = (Screen.width * cardPos / 100);
        float cardY = (Screen.height * (87.5f - index * 25) / 100);

        cardUnitGO.name = teamName + index;
        cardUnitGO.transform.SetParent(parentScene.transform, false);
        cardUnitGO.transform.position = new Vector3(cardX, cardY);

        spellGO.name = teamName + index + "Spell";
        spellGO.transform.SetParent(parentScene.transform, false);
        spellGO.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        spellGO.transform.localScale = new Vector3(3.0f, 3.0f, 0);

        Vector3 castButtonPosition = new Vector3(spellGO.transform.position.x + 135, spellGO.transform.position.y - 190);
        Vector3 cancelButtonPosition = new Vector3(spellGO.transform.position.x - 135, spellGO.transform.position.y - 190);

        GameObject castButton = CreateButton(teamName + index + "CastButton", "Cast", spellGO, castButtonPosition);
        castButton.GetComponent<Button>().onClick.AddListener(castButton.GetComponent<SpellController>().CastSpell);
        castButtons.Add(castButton);

        GameObject cancelButton = CreateButton(teamName + index + "CancelButton", "Cancel", spellGO, cancelButtonPosition);
        cancelButton.GetComponent<Button>().onClick.AddListener(SpellController.CloseSpell);

        UnitController cardUnitController = cardUnitGO.GetComponent<UnitController>();

        cardUnitController.setUp(cardID, spellGO);

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
        FindObjectOfType<UnitController>().onSpellDisplay += SpellDisplay;
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

        for (int j = 0; j < cardUnitController.getColorsCount(); j++) {
            GameObject manaBG = Instantiate(manaBGOriginal);
            manaBGOriginal.name = unitTeamName + index + "WhiteBG";

            tempPrev++;

            manaBG.name = unitTeamName + index + "ManaBG" + j;
            manaBG.transform.position = new Vector3(-74.0f, 70.0f);
            manaBG.transform.SetParent(bgAll.transform, false);
            manaBG.transform.SetSiblingIndex(tempPrev);

            int temp = cardUnitController.getColorsCount() - (j + 1);

            cardUnitController.setUpColors(manaBG, temp);

            foreach (Transform child in manaBG.transform) {
                Destroy(child.gameObject);
            }

            if (j == cardUnitController.getColorsCount() - 1) {
                foreach (Transform child in manaBGOriginal.transform) {
                    child.transform.SetParent(manaBG.transform, false);
                }
            }
        }
        Destroy(manaBGOriginal.gameObject);
    }

    private void removeOnZero() {
        List<GameObject> allUnits = playerTeam.Concat(enemyTeam).ToList();

        foreach (GameObject unitGO in allUnits) {
            UnitController unit = unitGO.GetComponent<UnitController>();

            int tempHealth = unit.GetHealth();
            state = BattleStateHandler.GetState();

            if (tempHealth <= 0) {
                if (state != BattleState.Won && state != BattleState.Lost) {
                    if (unitGO.name.Contains(ally)) {
                        playerTeam.Remove(unitGO);
                    }
                    else if (unitGO.name.Contains(enemy)) {
                        enemyTeam.Remove(unitGO);
                    }
                    Destroy(unitGO);
                }
            }
        }

        checkGameResult();
    }

    private void checkGameResult() {
        if (enemyTeam.Count == 0) {
            BattleStateHandler.setState(BattleState.Won);
        }
        else if (playerTeam.Count == 0) {
            BattleStateHandler.setState(BattleState.Lost);
        }
    }

    public List<GameObject> getPlayerTeam() {
        return playerTeam;
    }

    public List<GameObject> getEnemyTeam() {
        return enemyTeam;
    }
}