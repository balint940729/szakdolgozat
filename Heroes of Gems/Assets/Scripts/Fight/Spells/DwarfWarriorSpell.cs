using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DwarfWarriorSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOppenentTeam();

        caster.ModifyArmor(caster.GetSpellDamage());

        UnitController target = targetsGO.First().GetComponent<UnitController>();

        UnitController.NormalDamage(caster.GetArmor(), target);
    }
}