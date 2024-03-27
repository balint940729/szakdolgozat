public enum BattleState { Start, WaitingForPlayer, PlayerTurn, WaitingForEnemy, EnemyTurn, Won, Lost };

public static class BattleStateHandler {
    private static BattleState state;

    public static BattleState GetState() {
        return state;
    }

    public static BattleState SetState(BattleState battleState) {
        return state = battleState;
    }
}