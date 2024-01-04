using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarpySpell : SpellBaseClass {

    public HarpySpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.GainMana(-caster.GetSpellDamage());
        }
    }
}