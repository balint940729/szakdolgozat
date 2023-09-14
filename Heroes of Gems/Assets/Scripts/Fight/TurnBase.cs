using System.Collections.Generic;
using UnityEngine;

public class TurnBase : MonoBehaviour {

    // Card Prefab - Border, Attack, Health, Armor icons
    public GameObject cardPrefab;

    public Transform parentScene;

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

        SetUpTeam(true);
        SetUpTeam(false);
    }

    private void Update() {
        //if (state != BattleState.WON && state != BattleState.LOST) {
        //    if (Input.GetKeyDown(KeyCode.Space)) {
        //        UnitController player = playerTeam[0].GetComponent<UnitController>();
        //        UnitController enemy = enemyTeam[0].GetComponent<UnitController>();

        //        if (state == BattleState.PLAYERTURN) {
        //            player.Attack(enemy);
        //            removeOnZero(enemy);
        //        }
        //        else if (state == BattleState.ENEMYTURN) {
        //            enemy.Attack(player);
        //            removeOnZero(player);
        //        }
        //    }
        //}
    }

    private void Combat() {
        state = BattleStateHandler.GetState();
        if (state != BattleState.WON && state != BattleState.LOST) {
            UnitController player = playerTeam[0].GetComponent<UnitController>();
            UnitController enemy = enemyTeam[0].GetComponent<UnitController>();

            if (state == BattleState.PLAYERTURN) {
                player.Attack(enemy);
                removeOnZero(enemy);
            }
            else if (state == BattleState.ENEMYTURN) {
                enemy.Attack(player);
                removeOnZero(player);
            }
        }
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

        if (state == BattleState.PLAYERTURN) {
            if (tempHealth <= 0) {
                Destroy(enemyTeam[0]);
                enemyTeam.Remove(enemyTeam[0]);
            }

            BattleStateHandler.setState(BattleState.ENEMYTURN);

            //state = BattleState.ENEMYTURN;
        }
        else if (state == BattleState.ENEMYTURN) {
            tempHealth = unit.GetHealth();

            if (tempHealth <= 0) {
                Destroy(playerTeam[0]);
                playerTeam.Remove(playerTeam[0]);
            }

            BattleStateHandler.setState(BattleState.PLAYERTURN);
            //state = BattleState.PLAYERTURN;
        }

        checkGameResult();
    }

    private void checkGameResult() {
        if (enemyTeam.Count == 0) {
            BattleStateHandler.setState(BattleState.WON);
            //state = BattleState.WON;
        }
        else if (playerTeam.Count == 0) {
            BattleStateHandler.setState(BattleState.LOST);
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