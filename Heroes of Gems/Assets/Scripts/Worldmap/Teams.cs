using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Teams : MonoBehaviour, IDataPersistence {

    [SerializeField]
    private static List<TeamObjectData> teams = new List<TeamObjectData>();

    //public static List<Unit[]> teams = new List<Unit[]>()
    public static List<TeamObjectData> GetTeams() {
        return teams;
    }

    public static void AddTeam(Team team) {
        TeamObjectData teamGOD = new TeamObjectData(team.name, team.GetTeam(), team.IsSelected());
        teams.Add(teamGOD);
    }

    public void LoadData(GameData gameData) {
        teams.Clear();
        teams = gameData.teams;
    }

    public void SaveData(ref GameData gameData) {
        gameData.teams = teams;
    }
}