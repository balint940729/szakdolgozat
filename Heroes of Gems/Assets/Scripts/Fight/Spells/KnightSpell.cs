using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class KnightSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();

        if (target.GetRace().raceName != "Humans") {
            UnitController.NormalDamage(caster.GetSpellDamage() * 2, target);
            return;
        }

        UnitController.NormalDamage(caster.GetSpellDamage(), target);
    }
}