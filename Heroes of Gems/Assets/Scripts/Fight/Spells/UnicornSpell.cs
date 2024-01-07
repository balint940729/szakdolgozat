using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnicornSpell : SpellBaseClass {

    public UnicornSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetAllyTeam();

        //Get the first 3 element of the list
        for (int i = 0; i < 2; i++) {
            UnitController target = targetsGO.ElementAt(i).GetComponent<UnitController>();
            target.ModifyHealth(caster.GetSpellDamage());
            target.GainMana(3);
        }
    }
}