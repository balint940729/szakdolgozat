using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DwarfWarriorSpell : SpellBaseClass {

    public DwarfWarriorSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        caster.ModifyArmor(caster.GetSpellDamage());

        UnitController target = targetsGO.First().GetComponent<UnitController>();

        UnitController.NormalDamage(caster.GetArmor(), target);
    }
}