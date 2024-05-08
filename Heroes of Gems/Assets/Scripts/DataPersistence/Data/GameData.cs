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
    public List<EquipmentsObjectData> equipments;
    public List<CityObjectData> cities;
    public List<EnemyAlive> enemies;

    public GameData() {
        if (MainMenuScript.isNewGame) {
            gold = 200;
            units = new List<Unit>();
            items = new List<Item>();
        }
        GoldController.SetGold(gold);
        playerPos = Vector3.zero;
        teams = new List<TeamObjectData>();
    }
}