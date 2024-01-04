using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TreefolkSpell : SpellBaseClass {

    public TreefolkSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.Last().GetComponent<UnitController>();
        caster.ModifyHealth(caster.GetSpellDamage());
        caster.NormalDamage(caster.GetSpellDamage(), target);
    }
}