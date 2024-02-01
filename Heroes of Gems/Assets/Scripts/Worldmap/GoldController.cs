using UnityEngine;

public class GoldController : MonoBehaviour {
    private static GoldController instance;
    private static int gold;
    private int oldGoldValue;
    private GoldDisplay goldDisplay;

    private void Awake() {
        if (instance == null) {
            instance = this;
            gold = 200;
            oldGoldValue = gold;
        }
    }

    private void Start() {
        goldDisplay = GetComponent<GoldDisplay>();
        goldDisplay.ChangeGoldCounter();
    }

    public void Update() {
        if (oldGoldValue != gold) {
            goldDisplay.ChangeGoldCounter();
            oldGoldValue = gold;
        }
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