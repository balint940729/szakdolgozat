using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DogSpell : SpellBaseClass {

    public DogSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();
        caster.NormalDamage(caster.GetSpellDamage(), target);
    }
}