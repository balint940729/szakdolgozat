using UnityEditor;
using UnityEngine;

public class UnitController : MonoBehaviour {
    private UnitDisplay unitCard;
    private string folderPath = "Assets/Sprites/Cards";
    private string[] assetGuids;
    private int health;
    private int armor;

    // Start is called before the first frame update
    private void Awake() {
        unitCard = GetComponent<UnitDisplay>();
        assetGuids = AssetDatabase.FindAssets("t:Unit", new string[] { folderPath });
    }

    public void setUp(int cardID) {
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[cardID]);
        unitCard.card = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);

        health = unitCard.card.unitHealth;
        armor = unitCard.card.unitArmor;
    }

    public int GetHealth() {
        return health;
    }

    public int GetArmor() {
        return armor;
    }

    public void Attack(UnitController target) {
        target.armor -= unitCard.card.unitAttack;

        if (target.armor < 0) {
            target.health += target.armor;
            target.armor = 0;
        }

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.setHealth(target.health, target.armor);
    }
}