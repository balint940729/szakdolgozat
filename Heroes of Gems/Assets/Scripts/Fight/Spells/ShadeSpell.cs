using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShadeSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();

        int drainedLifeAmount = target.GetHealth() - caster.GetSpellDamage();

        if (drainedLifeAmount > 1) {
            caster.ModifyHealth(target.GetHealth());
        }
        else {
            caster.ModifyHealth(caster.GetSpellDamage());
        }

        UnitController.TrueDamage(caster.GetSpellDamage(), target);
    }
}