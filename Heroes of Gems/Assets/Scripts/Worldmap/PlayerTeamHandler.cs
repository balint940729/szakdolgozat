using UnityEngine;

public class PlayerTeamHandler : MonoBehaviour, IDataPersistence {
    [SerializeField]
    private static Unit[] team;

    public static Unit[] GetTeam() {
        return team;
    }

    public static void SetTeam(Unit[] team) {
        PlayerTeamHandler.team = team;
    }

    public void LoadData(GameData gameData) {
        SetTeam(gameData.playerTeam);
    }

    public void SaveData(ref GameData gameData) {
        gameData.playerTeam = team;
    }
}