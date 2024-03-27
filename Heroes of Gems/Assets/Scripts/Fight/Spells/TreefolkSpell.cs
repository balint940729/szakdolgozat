using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TreefolkSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        UnitController target = targetsGO.Last().GetComponent<UnitController>();
        caster.ModifyHealth(caster.GetSpellDamage());
        UnitController.NormalDamage(caster.GetSpellDamage(), target);
    }
}