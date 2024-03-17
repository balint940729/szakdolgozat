using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarpySpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.GainMana(-caster.GetSpellDamage());
        }
    }
}