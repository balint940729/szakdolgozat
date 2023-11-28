using UnityEngine;

public class SpellController : MonoBehaviour {
    private static bool isSpellDisplayed = false;
    private static GameObject displayedSpell;
    private static GameObject displayedUnit;

    public void CastSpell() {
        UnitController unitController = displayedUnit.GetComponent<UnitController>();
        TurnBase.GetInstance().CastSpell(unitController);
    }

    public static void CloseSpell() {
        if (isSpellDisplayed) {
            displayedSpell.SetActive(false);
            isSpellDisplayed = false;
        }
    }

    public static void ShowSpell(GameObject spell, GameObject unit) {
        if (!isSpellDisplayed) {
            displayedUnit = unit;
            displayedSpell = spell;
            displayedSpell.SetActive(true);
            isSpellDisplayed = true;
        }
    }
}