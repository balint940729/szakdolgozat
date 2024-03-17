using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Teams : MonoBehaviour, IDataPersistence {
    public static List<Team> teams = new List<Team>();

    private void Start() {
        DataPersistenceManager.AddDataPersistence(this);
    }

    public void LoadData(GameData gameData) {
        //foreach (Unit[] item in gameData.teams) {
        //    Team tempTeam = item;
        //}
        //teams = gameData.teams;
    }

    public void SaveData(ref GameData gameData) {
        //foreach (Team team in teams) {
        //    List<string> jsonUnits = new List<string>();
        //    foreach (Unit unit in team.team) {
        //        string unitJson = JsonUtility.ToJson(unit);
        //        jsonUnits.Add(unitJson);
        //    }

        //    gameData.teams.Add(jsonUnits.ToArray());
        //}
        //gameData.teams = teams;
    }
}