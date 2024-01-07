using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThiefSpell : SpellBaseClass {

    public ThiefSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.Last().GetComponent<UnitController>();
        UnitController.TrueDamage(caster.GetSpellDamage(), target);
    }
}