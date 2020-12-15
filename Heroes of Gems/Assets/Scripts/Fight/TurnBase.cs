using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST};

public class TurnBase : MonoBehaviour
{
    // Card Prefab - Border, Attack, Health, Armor icons
    public GameObject cardPrefab;

    // Scriptable Object where variable values are
    public ScriptableObject player1;
    public ScriptableObject player2;
    public ScriptableObject player3;
    public ScriptableObject player4;
    public ScriptableObject enemy1;
    public ScriptableObject enemy2;
    public ScriptableObject enemy3;
    public ScriptableObject enemy4;

    // CardPositions
    public Transform player1Pos;
    public Transform player2Pos;
    public Transform player3Pos;
    public Transform player4Pos;

    public Transform enemy1Pos;
    public Transform enemy2Pos;
    public Transform enemy3Pos;
    public Transform enemy4Pos;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
    }

    // Setup the Card to the Board
    void SetUpBattle() 
    {
        GameObject playerGO = Instantiate(cardPrefab, player1Pos);
        UnitDisplay player1Unit = playerGO.GetComponent<UnitDisplay>();
        player1Unit.card = (Unit)player1;

        GameObject player2GO = Instantiate(cardPrefab, player2Pos);
        UnitDisplay player2Unit = player2GO.GetComponent<UnitDisplay>();
        player2Unit.card = (Unit)player2;

        GameObject player3GO = Instantiate(cardPrefab, player3Pos);
        UnitDisplay player3Unit = player3GO.GetComponent<UnitDisplay>();
        player3Unit.card = (Unit)player3;

        GameObject player4GO = Instantiate(cardPrefab, player4Pos);
        UnitDisplay player4Unit = player4GO.GetComponent<UnitDisplay>();
        player4Unit.card = (Unit)player4;

        GameObject enemyGO = Instantiate(cardPrefab, enemy1Pos);
        UnitDisplay enemy1Unit = enemyGO.GetComponent<UnitDisplay>();
        enemy1Unit.card = (Unit)enemy1;

        GameObject enemy2GO = Instantiate(cardPrefab, enemy2Pos);
        UnitDisplay enemy2Unit = enemy2GO.GetComponent<UnitDisplay>();
        enemy2Unit.card = (Unit)enemy2;

        GameObject enemy3GO = Instantiate(cardPrefab, enemy3Pos);
        UnitDisplay enemy3Unit = enemy3GO.GetComponent<UnitDisplay>();
        enemy3Unit.card = (Unit)enemy3;

        GameObject enemy4GO = Instantiate(cardPrefab, enemy4Pos);
        UnitDisplay enemy4Unit = enemy4GO.GetComponent<UnitDisplay>();
        enemy4Unit.card = (Unit)enemy4;
    }
}
