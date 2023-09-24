using UnityEditor;
using UnityEngine;

public class UnitController : MonoBehaviour {
    private UnitDisplay unitCard;
    private string folderPath = "Assets/Sprites/Cards";
    private string[] assetGuids;
    private int health;
    private int armor;
    private int mana;
    private int maxMana;

    // Start is called before the first frame update
    private void Awake() {
        unitCard = GetComponent<UnitDisplay>();
        assetGuids = AssetDatabase.FindAssets("t:Unit", new string[] { folderPath });
    }

    public void setUp(int cardID) {
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[cardID]);
        unitCard.card = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);

        health = unitCard.card.baseHealth;
        armor = unitCard.card.baseArmor;
        mana = unitCard.card.currentMana;
        maxMana = unitCard.card.maxMana;
    }

    public int GetHealth() {
        return health;
    }

    public int GetArmor() {
        return armor;
    }

    public void Attack(UnitController target) {
        target.armor -= unitCard.card.baseAttack;

        if (target.armor < 0) {
            target.health += target.armor;
            target.armor = 0;
        }

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.setHealth(target.health, target.armor);
    }

    public int gainMana(int manaAmount) {
        int remainedMana = 0;
        mana += manaAmount;

        if (mana > maxMana) {
            remainedMana = mana - maxMana;
            mana = maxMana;
        }

        unitCard.setMana(mana);

        return remainedMana;
    }

    public bool isOnFullMana() {
        if (mana == maxMana) {
            return true;
        }

        return false;
    }
}