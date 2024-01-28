using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WerewolfSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();

        if (target.GetRace().raceName == "Humans") {
            target.ModifyAttack(-caster.GetSpellDamage() * 2);
            target.ModifyArmor(-caster.GetSpellDamage() * 2);
            UnitController.TrueDamage(caster.GetSpellDamage() * 2, target);
            return;
        }

        target.ModifyAttack(-caster.GetSpellDamage());
        target.ModifyArmor(-caster.GetSpellDamage());
        UnitController.TrueDamage(caster.GetSpellDamage(), target);
    }
}