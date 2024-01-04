using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArtificerSpell : SpellBaseClass {

    public ArtificerSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetAllyTeam();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();
            target.ModifyArmor(caster.GetSpellDamage());
        }
    }
}