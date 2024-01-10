using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EliteDwarfSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController strongestTarget = targetsGO[0].GetComponent<UnitController>();
        int strongestStat = strongestTarget.GetArmor() + strongestTarget.GetHealth();

        foreach (GameObject targetGO in targetsGO) {
            UnitController target = targetGO.GetComponent<UnitController>();

            if (strongestStat > (target.GetArmor() + target.GetHealth())) {
                strongestStat = target.GetArmor() + target.GetHealth();
                strongestTarget = target;
            }
        }

        caster.ModifyArmor(caster.GetSpellDamage());
        caster.ModifyHealth(caster.GetSpellDamage());
        caster.ModifyAttack(caster.GetSpellDamage());

        UnitController.NormalDamage(caster.GetArmor() + caster.GetAttack() + caster.GetHealth(), strongestTarget);
    }
}