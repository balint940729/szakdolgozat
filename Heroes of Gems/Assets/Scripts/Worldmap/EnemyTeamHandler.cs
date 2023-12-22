using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyTeamHandler {
    private static List<Unit> team;

    public static List<Unit> GetTeam() {
        return team;
    }

    public static void SetTeam(List<Unit> team) {
        EnemyTeamHandler.team = team;
    }

    public static void AddTeammember(Unit unit) {
        team.Add(unit);
    }
}