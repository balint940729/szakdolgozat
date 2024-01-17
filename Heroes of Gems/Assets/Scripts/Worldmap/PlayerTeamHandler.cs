using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerTeamHandler {
    private static Unit[] team;

    public static Unit[] GetTeam() {
        return team;
    }

    public static void SetTeam(Unit[] team) {
        PlayerTeamHandler.team = team;
    }

    //public static void AddTeammember(Unit unit) {
    //    team.
    //}
}