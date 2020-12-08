using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PLAYERTURN, ENEMYTURN, WON, LOST};

public class TurnBase : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private Unit playerUnit;
    private Unit enemyUnit;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        SetUpBattle();
    }

    void SetUpBattle() 
    {
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();


    }
}
