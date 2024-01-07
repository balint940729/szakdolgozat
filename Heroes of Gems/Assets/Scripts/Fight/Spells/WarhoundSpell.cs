using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarhoundSpell : SpellBaseClass {

    public WarhoundSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyAttack(-caster.GetSpellDamage());

            if (target.GetRace().raceName != "Undeads") {
                UnitController.NormalDamage(caster.GetSpellDamage(), target);
            }
        }
    }
}