using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BarbarianSpell : SpellBaseClass {

    public override void InitializeSpell() {
        caster.ModifyAttack(caster.GetSpellDamage());

        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();
        UnitController.NormalDamage(caster.GetAttack(), target);
    }
}