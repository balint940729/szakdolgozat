using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DruidSpell : SpellBaseClass {

    public DruidSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            caster.NormalDamage(caster.GetSpellDamage(), target);
        }
    }
}