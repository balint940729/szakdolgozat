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
        //foreach (TeamObjectData teamGOD in gameData.teams) {
        //    Team team = default;
        //    //team.gameObject.name = teamGOD.teamName;
        //    team.SetTeam(teamGOD.members);
        //    team.SetSelected(teamGOD.isSelected);

        //    teams.Add(team);
        //}
        //foreach (List<string> unitJsonList in gameData.teams) {
        //    Team team = new Team();
        //    team.team = new Unit[unitJsonList.Count];
        //    for (int i = 0; i < unitJsonList.Count; i++) {
        //        team.team[i] = JsonUtility.FromJson<Unit>(unitJsonList[i]);
        //    }
        //    teams.Add(team);
        //}
        //teams = gameData.teams;
    }

    public void SaveData(ref GameData gameData) {
        gameData.teams.Clear();
        //foreach (Team team in teams) {
        //    //Unit[] teamMembers = new Unit[team.team.members.Length];
        //    //for (int i = 0; i < team.team.members.Length; i++) {
        //    //    teamMembers[i] = team.team.members[i];
        //    //}
        //    TeamObjectData tempTeam = new TeamObjectData(team.gameObject.name, team.GetTeam(), team.IsSelected());
        //    gameData.teams.Add(tempTeam);

        //}
        gameData.teams = teams;
    }
}