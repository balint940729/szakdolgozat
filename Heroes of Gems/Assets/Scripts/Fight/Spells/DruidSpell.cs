using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DruidSpell : SpellBaseClass {

    public DruidSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

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