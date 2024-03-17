using UnityEngine;

public class GoldController : MonoBehaviour, IDataPersistence {
    private static GoldController instance;
    private static int gold;
    private static int oldGoldValue;
    private GoldDisplay goldDisplay;

    private void Awake() {
        if (instance == null) {
            instance = this;
            //Add the persistance, because initially is not active
            DataPersistenceManager.AddDataPersistence(this);
        }
    }

    //private void Start() {
    //    goldDisplay = GetComponent<GoldDisplay>();
    //    goldDisplay.ChangeGoldCounter();
    //}

    //public void Update() {
    //    if (oldGoldValue != gold) {
    //        goldDisplay.ChangeGoldCounter();
    //        oldGoldValue = gold;
    //    }
    //}

    public void LoadData(GameData gameData) {
        gold = gameData.gold;
        oldGoldValue = gold;
    }

    public void SaveData(ref GameData gameData) {
        gameData.gold = GetGold();
    }

    public static bool IsGoldChanged() {
        if (oldGoldValue != gold) {
            oldGoldValue = gold;
            return true;
        }
        return false;
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