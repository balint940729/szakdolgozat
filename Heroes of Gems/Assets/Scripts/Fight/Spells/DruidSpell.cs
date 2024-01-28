using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DruidSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        caster.ModifyHealth(caster.GetSpellDamage());
        caster.ModifyArmor(caster.GetSpellDamage());
        caster.ModifyAttack(caster.GetSpellDamage());

        UnitController target = targetsGO.First().GetComponent<UnitController>();
        if (target.GetRace().raceName == "Beasts") {
            UnitController.NormalDamage(caster.GetSpellDamage() * 2, target);
            return;
        }

        UnitController.NormalDamage(caster.GetSpellDamage(), target);
    }
}