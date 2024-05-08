using UnityEngine;

public class GoldController : MonoBehaviour, IDataPersistence {
    private static GoldController instance;
    private static int gold;

    private void Awake() {
        if (instance == null) {
            instance = this;
            //Add the persistance, because initially is not active
            DataPersistenceManager.AddDataPersistence(this);
        }
    }

    public void LoadData(GameData gameData) {
        gold = gameData.gold;
    }

    public void SaveData(ref GameData gameData) {
        gameData.gold = GetGold();
    }

    public static int GetGold() {
        return gold;
    }

    public static void SetGold(int amount) {
        gold = amount;
    }

    public static void AddGold(int amount) {
        gold += amount;
    }

    public static void SpendGold(int amount) {
        gold -= amount;
    }

    public static bool HasEnoughGold(int amount) {
        int remainedGold = gold - amount;

        if (remainedGold >= 0) {
            return true;
        }

        return false;
    }
}