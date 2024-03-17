using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public int gold;

    public Vector3 playerPos;

    public Unit[] playerTeam;
    public List<Unit[]> teams;
    public List<Unit> units;

    public GameData() {
        if (MainMenuScript.isNewGame) {
            gold = 200;
            playerTeam = new Unit[4];
        }

        GoldController.SetGold(gold);
        playerPos = Vector3.zero;
        teams = new List<Unit[]>();
        units = new List<Unit>();
        //teams = new List<Team>();
    }
}