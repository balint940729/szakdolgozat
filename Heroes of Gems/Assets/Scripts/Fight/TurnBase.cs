using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST };

public class TurnBase : MonoBehaviour {

    // Card Prefab - Border, Attack, Health, Armor icons
    public GameObject cardPrefab;

    public Transform parentScene;

    public BattleState state;

    private List<UnitController> playerTeam = new List<UnitController>();
    private List<UnitController> enemyTeam = new List<UnitController>();
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
            UnitController player = playerTeam[0];
            UnitController enemy = enemyTeam[0];

            player.Attack(enemy);
        }
    }

    // Setup the Card to the Board
    private void SetUpTeam(bool isPlayerTeam) {
        float posX;
        float posY;

        parentScene = GameObject.Find("GameCanvas").transform;

        for (int i = 0; i < 4; i++) {
            if (isPlayerTeam) {
                posX = 720;
                posY = 405 - (i * 270);
                GameObject cardUnitGO = Instantiate(cardPrefab);
                cardUnitGO.name = "Ally" + i;
                cardUnitGO.transform.position = new Vector3(posX, posY);
                cardUnitGO.transform.SetParent(parentScene.transform, false);

                UnitController cardUnitController = cardUnitGO.GetComponent<UnitController>();

                cardUnitController.setUp(0);
                playerTeam.Add(cardUnitController);
            }
            else {
                posX = -720;
                posY = 405 - (i * 270);
                GameObject cardUnitGO = Instantiate(cardPrefab);
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

                enemyTeam.Add(cardUnitController);
            }
        }
    }
}