using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighElfSpell : SpellBaseClass {

    public HighElfSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetAllyTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyHealth(caster.GetSpellDamage());
        }
    }
}