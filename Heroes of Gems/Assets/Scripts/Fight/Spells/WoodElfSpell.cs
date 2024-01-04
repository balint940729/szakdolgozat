using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WoodElfSpell : SpellBaseClass {

    public WoodElfSpell(string spellName, string spellDescription, Sprite spellImage) : base(spellName, spellDescription, spellImage) {
    }

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController weakestTarget = targetsGO[0].GetComponent<UnitController>();
        int weakestStat = weakestTarget.GetArmor() + weakestTarget.GetHealth();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();

            if (weakestStat > (target.GetArmor() + target.GetHealth())) {
                weakestStat = target.GetArmor() + target.GetHealth();
                weakestTarget = target;
            }
        }

        caster.NormalDamage(caster.GetSpellDamage(), weakestTarget);
    }
}