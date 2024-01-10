using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MerfolkSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();
        List<GameObject> alliesGO = GetAllyTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifySpellDamage(-caster.GetSpellDamage());
        }

        foreach (GameObject allyGO in alliesGO) {
            UnitController target = allyGO.GetComponent<UnitController>();
            target.ModifySpellDamage(caster.GetSpellDamage());
        }
    }
}