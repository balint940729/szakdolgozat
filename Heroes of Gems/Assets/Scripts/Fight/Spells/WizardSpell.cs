using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WizardSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            UnitController.NormalDamage(caster.GetSpellDamage(), target);
        }
    }
}