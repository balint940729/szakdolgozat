using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WerewolfSpell : SpellBaseClass {

    public WerewolfSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

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