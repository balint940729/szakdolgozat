using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerTeamHandler {
    private static List<Unit> team;

    public static List<Unit> GetTeam() {
        return team;
    }

    public static void SetTeam(List<Unit> team) {
        PlayerTeamHandler.team = team;
    }

    public static void AddTeammember(Unit unit) {
        team.Add(unit);
    }
}