using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PLAYERTURN, ENEMYTURN, WON, LOST};

public class TurnBase : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        SetUpBattle();
    }

    void SetUpBattle() 
    {
        Instantiate(playerPrefab);
        Instantiate(enemyPrefab);
    }
}
