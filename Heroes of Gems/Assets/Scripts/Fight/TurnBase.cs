using System.Collections.Generic;
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

    private void turnChange() {
        state = BattleStateHandler.GetState();
        if (state != BattleState.Won && state != BattleState.Lost) {
            if (state == BattleState.WaitingForPlayer) {
                turnArrowGO.transform.position = new Vector3(155, 680);
            }
            else if (state == BattleState.WaitingForEnemy) {
                turnArrowGO.transform.position = new Vector3(1075, 680);
            }
        }
    }

    private void gainMana() {
        state = BattleStateHandler.GetState();
        int manaAmount = 3;

        if (state != BattleState.Won && state != BattleState.Lost) {
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

                extraMana += unitCntr.gainMana(manaGained);

                if (extraMana == 0) {
                    break;
                }

                manaGained = extraMana;
            }
        }
    }

    private void Combat() {
        state = BattleStateHandler.GetState();
        if (state != BattleState.Won && state != BattleState.Lost) {
            UnitController player = playerTeam[0].GetComponent<UnitController>();
            UnitController enemy = enemyTeam[0].GetComponent<UnitController>();

            if (state == BattleState.PlayerTurn) {
                player.Attack(enemy);
                removeOnZero(enemy);
            }
            else if (state == BattleState.EnemyTurn) {
                enemy.Attack(player);
                removeOnZero(player);
            }
        }
    }

    private void SetUpBoard() {
        parentScene = GameObject.Find("GameCanvas").transform;

        turnArrowGO = Instantiate(turnPrefab);

        turnArrowGO.name = "TurnArrow";
        turnArrowGO.transform.SetParent(parentScene.transform, false);
        turnArrowGO.transform.position = new Vector3(155, 680);
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

                cardUnitGO.name = "Ally" + i;
                cardUnitGO.transform.position = new Vector3(posX, posY);
                cardUnitGO.transform.SetParent(parentScene.transform, false);

                UnitController cardUnitController = cardUnitGO.GetComponent<UnitController>();

                cardUnitController.setUp(0);
                playerTeam.Add(cardUnitGO);
            }
            else {
                posX = 720;
                posY = 405 - (i * 270);
                cardUnitGO.name = "Enemy" + i;
                cardUnitGO.transform.position = new Vector3(posX, posY);
                cardUnitGO.transform.SetParent(parentScene.transform, false);

                UnitController cardUnitController = cardUnitGO.GetComponent<UnitController>();

                if (i == 1) {
                    cardUnitController.setUp(1);
                }
                else {
                    cardUnitController.setUp(2);
                }

                enemyTeam.Add(cardUnitGO);
            }
        }
    }

    private void removeOnZero(UnitController unit) {
        int tempHealth = unit.GetHealth();
        state = BattleStateHandler.GetState();

        if (tempHealth <= 0) {
            if (state == BattleState.PlayerTurn) {
                Destroy(enemyTeam[0]);
                enemyTeam.Remove(enemyTeam[0]);
            }
            else if (state == BattleState.EnemyTurn) {
                Destroy(playerTeam[0]);
                playerTeam.Remove(playerTeam[0]);
            }
        }

        checkGameResult();
    }

    private void checkGameResult() {
        if (enemyTeam.Count == 0) {
            BattleStateHandler.setState(BattleState.Won);
            //state = BattleState.WON;
        }
        else if (playerTeam.Count == 0) {
            BattleStateHandler.setState(BattleState.Lost);
            //state = BattleState.LOST;
        }
    }

    public List<GameObject> getPlayerTeam() {
        return playerTeam;
    }

    public List<GameObject> getEnemyTeam() {
        return enemyTeam;
    }
}