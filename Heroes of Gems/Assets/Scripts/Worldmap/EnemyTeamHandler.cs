﻿public static class EnemyTeamHandler {
    private static Unit[] team;

    public static Unit[] GetTeam() {
        return team;
    }

    public static void SetTeam(Unit[] team) {
        EnemyTeamHandler.team = team;
    }
}