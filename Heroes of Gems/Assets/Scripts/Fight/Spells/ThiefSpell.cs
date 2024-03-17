using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ThiefSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        UnitController target = targetsGO.Last().GetComponent<UnitController>();
        UnitController.TrueDamage(caster.GetSpellDamage(), target);
    }
}