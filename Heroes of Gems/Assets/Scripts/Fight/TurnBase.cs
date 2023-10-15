using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnBase : MonoBehaviour {

    // Card Prefab - Border, Attack, Health, Armor icons
    public GameObject cardPrefab;

    public GameObject turnPrefab;

    public Transform parentScene;
    private GameObject turnArrowGO;

    private List<GameObject> playerTeam = new List<GameObject>();
    private List<GameObject> enemyTeam = new List<GameObject>();
    private static TurnBase instance;

    private BattleState state;

    private const string ally = "Ally";
    private const string enemy = "Enemy";

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
        FindObjectOfType<Match3>().turnChangeTriggered += turnChange;

        SetUpTeam(true);
        SetUpTeam(false);

        SetUpBoard();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (BattleStateHandler.GetState() != BattleState.Won && BattleStateHandler.GetState() != BattleState.Lost) {
                UnitController player = playerTeam[0].GetComponent<UnitController>();
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

                player.castSpell(allyTargets, targets);
                removeOnZero();
            }
        }
    }

    private void turnChange() {
        state = BattleStateHandler.GetState();
        if (state != BattleState.Won && state != BattleState.Lost) {
            if (state == BattleState.WaitingForPlayer) {
                turnArrowGO.transform.position = new Vector3(155, 675);
            }
            else if (state == BattleState.WaitingForEnemy) {
                turnArrowGO.transform.position = new Vector3(1075, 675);
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
        parentScene = GameObject.Find("GameCanvas").transform;

        turnArrowGO = Instantiate(turnPrefab);

        turnArrowGO.name = "TurnArrow";
        turnArrowGO.transform.SetParent(parentScene.transform, false);
        turnArrowGO.transform.position = new Vector3(155, 675);
        turnArrowGO.transform.SetAsLastSibling();
    }

    // Setup the Card to the Board
    private void SetUpTeam(bool isPlayerTeam) {
        float posX;
        float posY;

        parentScene = GameObject.Find("GameCanvas").transform;

        for (int i = 0; i < 4; i++) {
            GameObject cardUnitGO = Instantiate(cardPrefab);

            if (isPlayerTeam) {
                posX = -720;
                posY = 405 - (i * 270);

                cardUnitGO.name = ally + i;
                cardUnitGO.transform.position = new Vector3(posX, posY);
                cardUnitGO.transform.SetParent(parentScene.transform, false);

                UnitController cardUnitController = cardUnitGO.GetComponent<UnitController>();

                cardUnitController.setUp(0);

                SetUpUnitColor(isPlayerTeam, cardUnitController, i);
                playerTeam.Add(cardUnitGO);
            }
            else {
                posX = 720;
                posY = 405 - (i * 270);
                cardUnitGO.name = enemy + i;
                cardUnitGO.transform.position = new Vector3(posX, posY);
                cardUnitGO.transform.SetParent(parentScene.transform, false);

                UnitController cardUnitController = cardUnitGO.GetComponent<UnitController>();

                if (i == 1) {
                    cardUnitController.setUp(1);
                }
                else {
                    cardUnitController.setUp(2);
                }

                SetUpUnitColor(isPlayerTeam, cardUnitController, i);

                enemyTeam.Add(cardUnitGO);
            }
        }
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
        List<GameObject> allUnits = new List<GameObject>();

        allUnits = playerTeam.Concat(enemyTeam).ToList();

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