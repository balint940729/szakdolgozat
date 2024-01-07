using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarlordSpell : SpellBaseClass {

    public WarlordSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        caster.ModifyAttack(caster.GetSpellDamage());
        caster.ModifySpellDamage(caster.GetSpellDamage());

        int randomIndex = Random.Range(0, targetsGO.Count);
        UnitController target = targetsGO.ElementAt(randomIndex).GetComponent<UnitController>();

        UnitController.NormalDamage(caster.GetSpellDamage(), target);
    }
}