using UnityEditor;
using UnityEngine;

public class UnitController : MonoBehaviour {
    private UnitDisplay unitCard;

    // Start is called before the first frame update
    private void Awake() {
        unitCard = GetComponent<UnitDisplay>();
    }

    public void setUp(int cardID) {
        string folderPath = "Assets/Sprites/Cards";
        string[] assetGuids = AssetDatabase.FindAssets("t:Unit", new string[] { folderPath });

        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[cardID]);
        unitCard.card = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);
    }

    public void Attack(UnitController targetController) {
        int tempHealth;
        int tempArmor;

        tempArmor = targetController.unitCard.card.unitHealth;
        tempHealth = targetController.unitCard.card.unitHealth;

        tempArmor -= unitCard.card.unitAttack;

        if (tempArmor < 0) {
            tempHealth += tempArmor;
            tempArmor = 0;
        }

        if (tempHealth < 0) {
            tempHealth = 0;
        }

        targetController.unitCard.showDamage(tempHealth, tempArmor);
    }
}