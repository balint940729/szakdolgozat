using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public int gold;

    public Vector3 playerPos;

    public Unit[] playerTeam;
    public List<TeamObjectData> teams;

    public List<Unit> units;

    public List<Item> items;
    public List<Item> equipments;
    public List<EnemyAlive> enemies;

    public GameData() {
        if (MainMenuScript.isNewGame) {
            gold = 200;
            playerTeam = new Unit[4];
            units = new List<Unit>();
            items = new List<Item>();
        }
        //enemies = new List<EnemyAlive>();
        GoldController.SetGold(gold);
        playerPos = Vector3.zero;
        teams = new List<TeamObjectData>();
    }
}