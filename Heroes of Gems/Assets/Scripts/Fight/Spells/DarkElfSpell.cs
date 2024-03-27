using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DarkElfSpell : SpellBaseClass {

    public override void InitializeSpell() {
        List<GameObject> targetsGO = GetOpponentTeam();

        UnitController target = targetsGO.First().GetComponent<UnitController>();
        if (target.GetColors().Find(color => color.colorName == "Yellow") != null) {
            UnitController.TrueDamage(caster.GetSpellDamage() * 2, target);
            return;
        }

        UnitController.TrueDamage(caster.GetSpellDamage(), target);
    }
}