using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SalamanderSpell : SpellBaseClass {

    public SalamanderSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            if (target.GetColors().Find(color => color.colorName == "Green") != null) {
                caster.NormalDamage(caster.GetSpellDamage() * 2, target);
                continue;
            }
            caster.NormalDamage(caster.GetSpellDamage(), target);
        }
    }
}