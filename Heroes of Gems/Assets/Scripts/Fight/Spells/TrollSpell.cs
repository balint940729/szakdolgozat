using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrollSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyArmor(-caster.GetSpellDamage());
        }
    }
}