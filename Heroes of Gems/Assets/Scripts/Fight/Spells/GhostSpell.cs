using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            UnitController.TrueDamage(caster.GetSpellDamage(), target);
        }
    }
}