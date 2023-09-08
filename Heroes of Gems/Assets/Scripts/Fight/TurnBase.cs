using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST };

public class TurnBase : MonoBehaviour {

    // Card Prefab - Border, Attack, Health, Armor icons
    public GameObject cardPrefab;

    public Transform parentScene;

    public BattleState state;

    private List<GameObject> playerTeam = new List<GameObject>();
    private List<GameObject> enemyTeam = new List<GameObject>();
    private static TurnBase instance;

    //public

    public static TurnBase GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start() {
        state = BattleState.START;

        SetUpTeam(true);
        SetUpTeam(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject playerGO = playerTeam[0];
            UnitController player = playerGO.GetComponent<UnitController>();
            GameObject enemyGO = enemyTeam[0];
            UnitController enemy = enemyGO.GetComponent<UnitController>();

            player.Attack(enemy);

            int enemyHealth = enemy.GetHealth();
            if (enemyHealth <= 0) {
                enemyTeam.Remove(enemyGO);
                Destroy(enemyGO);
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
}