using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmeraldDragonSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> enemyTargetsGO = GetOpponentTeam();
        List<GameObject> alliesGO = GetAllyTeam();

        int spellDamage = caster.GetSpellDamage();
        int redAlly = 0;

        foreach (GameObject allyGO in alliesGO) {
            UnitController ally = allyGO.GetComponent<UnitController>();
            if (ally.GetColors().Find(color => color.colorName == "Red") != null) {
                redAlly++;
            }
        }

        foreach (GameObject targetGO in enemyTargetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            UnitController.NormalDamage(caster.GetSpellDamage() * (redAlly > 1 ? redAlly - 1 : 1), target);
        }
    }
}